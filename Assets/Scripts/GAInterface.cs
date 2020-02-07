using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GAInterface : MonoBehaviour
{
    [Header("Genetic Algorithm Parameters")]
    public int RandomSeed;
    public int PopulationSize;
    public float MutationRate;
    public float DurationPerGeneration;

    [Header("Ball Chromosome Thresholds")]
    public float UpwardForceThreshold;
    public float ForwardForceThreshold;

    [Header("Game Object References")]
    public PlayerSpawner PlayerSpawnerRef;
    public Hole BasketTarget;

    private GeneticAlgorithm BasketballGA;
    private CooldownTimer EvaluationTimer;
    private float GenerationNumber = 0f, BallsScored = 0f, AverageFitness = 0f;

    public class UnityEventUI : UnityEvent<float> { };
    private Dictionary<string, UnityEventUI> UIEvents;

    void Awake()
    {
        UIEvents = new Dictionary<string, UnityEventUI>();
    }

    void Start()
    {
        if (BasketTarget)
            BasketTarget.OnBallCollision.AddListener(BallCollideWithBasket);
    }

    private void BallCollideWithBasket()
    {
        BallsScored++;
        TriggerUIEvent(UIEventKeys.KEY_SCORE, BallsScored);
    }

    public void AddUIEvent(string eventKey, UnityAction<float> uiEvent)
    {
        if (UIEvents != null)
        {
            if (!UIEvents.ContainsKey(eventKey))
                UIEvents.Add(eventKey, new UnityEventUI());
            UIEvents[eventKey].AddListener(uiEvent);
        }
    }

    public void TriggerUIEvent(string eventKey, float value)
    {
        if (UIEvents != null)
            if (UIEvents.ContainsKey(eventKey))
                UIEvents[eventKey].Invoke(value);
    }

    public void StartGAEvaluations()
    {
        if (EvaluationTimer == null)
        {
            if ((PlayerSpawnerRef) && (BasketTarget))
            {
                BasketballGA = new GeneticAlgorithm(PopulationSize, MutationRate, UpwardForceThreshold, ForwardForceThreshold);
                BasketballGA.InitialisePopulation();

                PlayerSpawnerRef.SpawnPlayer();
                PlayerSpawnerRef.SetPlayerLookTarget(BasketTarget.transform);
                PlayerSpawnerRef.SpawnBalls(PopulationSize, BasketTarget.transform);
                SetupBallsUsingChromosomeData();

                EvaluationTimer = new CooldownTimer(DurationPerGeneration);
                EvaluationTimer.OnCooldownPoll.AddListener(EvaluationComplete);
                EvaluationTimer.Reset();
            }
        }
    }

    private void SetupBallsUsingChromosomeData()
    {
        for (int ballIndex = 0; ballIndex < PopulationSize; ballIndex++)
        {
            Chromosome aChromosome = BasketballGA.GetChromosomeAtPosition(ballIndex);
            PlayerSpawnerRef.ApplyForceToSpecificBall(aChromosome.UpwardForce, aChromosome.ForwardForce, ballIndex);
        }

        GenerationNumber++;
        TriggerUIEvent(UIEventKeys.KEY_GENERATIONNUMBER, GenerationNumber);
    }

    private void EvaluationComplete()
    {
        EvaluateFitness();

        BasketballGA.EvolvePopulation(out AverageFitness);
        TriggerUIEvent(UIEventKeys.KEY_AVG_FITNESS, AverageFitness);

        ResetBasketballCourt();
        SetupBallsUsingChromosomeData();
    }

    private void EvaluateFitness()
    {
        for (int ballIndex = 0; ballIndex < PopulationSize; ballIndex++)
        {
            BallController ballController;
            if (PlayerSpawnerRef.TryGetBallAtSpecificIndex(ballIndex, out ballController))
            {
                float fitness = 1 - NormaliseValue(Mathf.Pow(1 / ballController.GetMinDistanceToTarget(), 2));
                fitness = ballController.IsScored ? fitness + 5 : fitness;
                BasketballGA.UpdateChromosomeFitness(fitness, ballIndex);
            }
        }
    }

    private float NormaliseValue(float value)
    {
        return (1 + value / (1 + Mathf.Abs(value))) * 0.5f;
    }

    private void ResetBasketballCourt()
    {
        BallsScored = -1f;
        BallCollideWithBasket();

        PlayerSpawnerRef.ResetPlayerBalls();
        EvaluationTimer.Reset();
    }

    void Update()
    {
        if (EvaluationTimer != null)
            EvaluationTimer.Update();
    }
}
