using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component which contains the functional logic for handling collisions between
/// ball game objects and this hole ("hoop") game object.
/// </summary>
public class Hole : MonoBehaviour
{
    public Transform ScoreSpawnerTransform;          // Reference to transform where balls are teleported if scored, collides with this hole game object.
    public UnityEvent OnBallCollision;      // Unity event which is invoked when a ball collides with this hole game object.

    void Awake()
    {
        OnBallCollision = new UnityEvent();
    }

    void OnTriggerEnter(Collider other)
    {
        if (OnBallCollision != null)
        {
            if (ScoreSpawnerTransform)
            {
                OnBallCollision.Invoke();

                // Update ball score status + reset ball position to specified spawner
                BallController otherBallController = other.GetComponent<BallController>();
                otherBallController.IsScored = true;
                otherBallController.ResetVelocity();
                otherBallController.SetPosition(ScoreSpawnerTransform.transform.position);
            }
        }
    }
}
