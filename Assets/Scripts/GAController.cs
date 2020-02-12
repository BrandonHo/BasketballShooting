using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component which facilitates the use of a genetic algorithm for
/// identifying successful basketball shots in the basketball court.
/// </summary>
public class GAController : MonoBehaviour
{
    [Header("Genetic Algorithm Parameters")]
    public int RandomSeed;                                                      // Seed used for the RNG.
    public int PopulationSize;                                                  // Population size used in the genetic algorithm.
    public float MutationRate;                                                  // Probability of mutations occuring for newly-bred, child chromosomes.
    public float DurationBeforeEvolution;                                       // Duration per generation before the population is evolved.

    [Header("Ball Chromosome Thresholds")]
    public float UpwardForceThreshold;                                          // Threshold value for randomising the upward force property in chromosomes.
    public float ForwardForceThreshold;                                         // Threshold value for randomising the forward force property in chromosomes.

    [Header("Game Object References")]
    public PlayerSpawner SpawnerForPlayer;                                      // Reference to the player spawner, used to spawn player and control the spawned player.
    public Hole BasketTarget;                                                   // Reference to the basketball hoop, used for chromosome fitness calculations.

    private GeneticAlgorithm BasketballGA;                                      // Genetic algorithm used for evolving solutions.
    private StopwatchTimer EvaluationTimer;                                     // Timer used for deciding when populations are evolved.
    private float GenerationNumber = 0f, BallsScored = 0f, AverageFitness = 0f; // Counters describing current state of the GA, used for UI purposes.

    public class UnityEventUI : UnityEvent<float> { };                          // Custom unity event which takes in a float parameter.
    private Dictionary<string, UnityEventUI> KeyToUIEventMap;                   // Map from event key to a UI event.

    void Awake()
    {
        KeyToUIEventMap = new Dictionary<string, UnityEventUI>();
    }

    /// <summary>
    /// Utility method for listening to a UI-related unity event.
    /// </summary>
    /// <param name="eventKey">Unique key associated with a UI-related unity event.</param>
    /// <param name="unityAction">Action be performed when the specified UI-related unity event occurs.</param>
    public void AddUIEvent(string eventKey, UnityAction<float> unityAction)
    {
        if (KeyToUIEventMap != null)
        {
            if (!KeyToUIEventMap.ContainsKey(eventKey))
                KeyToUIEventMap.Add(eventKey, new UnityEventUI());
            KeyToUIEventMap[eventKey].AddListener(unityAction);
        }
    }

    /// <summary>
    /// Utility method used for triggering a UI-related unity event.
    /// </summary>
    /// <param name="eventKey">Unique key associated with a UI-related unity event.</param>
    /// <param name="value">Value which is sent as an argument to listening actions.</param>
    public void TriggerUIEvent(string eventKey, float value)
    {
        if (KeyToUIEventMap != null)
            if (KeyToUIEventMap.ContainsKey(eventKey))
                KeyToUIEventMap[eventKey].Invoke(value);
    }

    void Start()
    {
        if (BasketTarget)
            BasketTarget.OnBallCollision.AddListener(BallCollideWithBasket);
    }

    /// <summary>
    /// Callback method for when a basketball successfully collides with the basketball hoop.
    /// </summary>
    private void BallCollideWithBasket()
    {
        // Update score + UI
        BallsScored++;
        TriggerUIEvent(UIEventKeys.KEY_SCORE, BallsScored);
    }

    /// <summary>
    /// Primary utility method which initialises the genetic algorithm and begins evolving
    /// solutions in the genetic algorithm.
    /// </summary>
    public void StartGAEvaluations()
    {
        if (EvaluationTimer == null)
        {
            if ((SpawnerForPlayer) && (BasketTarget))
            {
                // Initialise genetic algorithm
                BasketballGA = new GeneticAlgorithm(PopulationSize, MutationRate, UpwardForceThreshold, ForwardForceThreshold);
                BasketballGA.InitialisePopulation();

                // Spawn player and balls + setup balls with chromosome data
                SpawnerForPlayer.SpawnPlayer();
                SpawnerForPlayer.SetPlayerLookTarget(BasketTarget.transform);
                SpawnerForPlayer.SpawnBalls(PopulationSize, BasketTarget.transform);
                SetupBallsUsingChromosomeData();

                // Start timer, assign action once evaluation complete
                EvaluationTimer = new StopwatchTimer(DurationBeforeEvolution);
                EvaluationTimer.OnTimerPoll.AddListener(EvaluationComplete);
                EvaluationTimer.Reset();
            }
        }
    }

    /// <summary>
    /// Helper method which performs the setup of balls using chromosome data from the genetic algorithm.
    /// </summary>
    private void SetupBallsUsingChromosomeData()
    {
        // Setup each ball with chromosome data
        for (int ballIndex = 0; ballIndex < PopulationSize; ballIndex++)
        {
            Chromosome aChromosome = BasketballGA.GetChromosomeAtIndex(ballIndex);
            SpawnerForPlayer.ApplyForceToSpecificBall(aChromosome.UpwardForce, aChromosome.ForwardForce, ballIndex);
        }

        // Update UI
        GenerationNumber++;
        TriggerUIEvent(UIEventKeys.KEY_GENERATIONNUMBER, GenerationNumber);
    }

    /// <summary>
    /// Callback method which is performed to evolve the population.
    /// </summary>
    private void EvaluationComplete()
    {
        EvaluateFitnessOfChromosomes();
        BasketballGA.EvolvePopulation(out AverageFitness);
        TriggerUIEvent(UIEventKeys.KEY_AVG_FITNESS, AverageFitness);

        // Reset court + setup balls with evolved population data
        ResetBasketballCourt();
        SetupBallsUsingChromosomeData();
    }

    /// <summary>
    /// Helper method used to calculate the fitness of the population.
    /// </summary>
    private void EvaluateFitnessOfChromosomes()
    {
        for (int ballIndex = 0; ballIndex < PopulationSize; ballIndex++)
        {
            BallController ballController;
            if (SpawnerForPlayer.TryGetBallAtSpecificIndex(ballIndex, out ballController))
            {
                // High value if close, low value if far (value in (0, 1))
                float fitness = 1 - NormaliseValue(Mathf.Pow(1 / ballController.GetMinDistanceToTarget(), 2));

                // Scored status greatly contributes to fitness (5)
                fitness = ballController.IsScored ? fitness + 5 : fitness;
                BasketballGA.UpdateChromosomeFitness(fitness, ballIndex);
            }
        }
    }

    /// <summary>
    /// Helper method used to normalise/squash value to a (0, 1) range.
    /// </summary>
    /// <param name="value">Value to be normalised.</param>
    /// <returns>Normalised value within the (0, 1) range.</returns>
    private float NormaliseValue(float value)
    {
        return (1 + value / (1 + Mathf.Abs(value))) * 0.5f;
    }

    /// <summary>
    /// Helper method used to reset game objects within the basketball court.
    /// </summary>
    private void ResetBasketballCourt()
    {
        // Reset score counter
        BallsScored = -1f;
        BallCollideWithBasket();

        // Reset balls + timer for chromosome evaluations
        SpawnerForPlayer.ResetPlayerBalls();
        EvaluationTimer.Reset();
    }

    void Update()
    {
        if (EvaluationTimer != null)
            EvaluationTimer.Update();
    }
}
