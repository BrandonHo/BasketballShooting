using UnityEngine;
using UnityEngine.Events;

public class Hole : MonoBehaviour
{
    public Transform ScoreSpawner;
    public UnityEvent OnBallCollision;

    void Awake()
    {
        OnBallCollision = new UnityEvent();
    }

    void OnTriggerEnter(Collider other)
    {
        if (OnBallCollision != null)
        {
            if (ScoreSpawner)
            {
                OnBallCollision.Invoke();
                other.GetComponent<BallController>().IsScored = true;
                other.GetComponent<BallController>().ResetToPosition(ScoreSpawner.transform.position);
            }
        }
    }
}
