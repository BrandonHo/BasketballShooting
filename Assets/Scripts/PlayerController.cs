using UnityEngine;

/// <summary>
/// Component used for managing and controlling the player game object.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public GameObject BallPrefab;                       // Prefab used for generating ball game objects
    public Transform BallSpawnerTransform;              // Transform used to position newly-instantiated ball game objects, and for parenting the ball game objects
    private GameObject[] SpawnedBalls;                  // Array containing references to the ball game objects
    private BallController[] SpawnedBallControllers;    // Array containing references to the ball controllers associated with ball game objects

    /// <summary>
    /// Utility method for spawning balls by the player.
    /// </summary>
    /// <param name="numberOfBalls">Number of balls to be spawned associated with the player game object.</param>
    /// <param name="target">Target transform used for ball-related calculations.</param>
    public void SpawnBalls(int numberOfBalls, Transform target)
    {
        if ((BallPrefab) && (BallSpawnerTransform))
        {
            SpawnedBalls = new GameObject[numberOfBalls];
            SpawnedBallControllers = new BallController[numberOfBalls];

            for (int ballIndex = 0; ballIndex < numberOfBalls; ballIndex++)
            {
                SpawnedBalls[ballIndex] = Instantiate(BallPrefab, BallSpawnerTransform.position, transform.rotation, transform);
                SpawnedBallControllers[ballIndex] = SpawnedBalls[ballIndex].GetComponent<BallController>();
                SpawnedBallControllers[ballIndex].SetTarget(target);
            }
        }
    }

    /// <summary>
    /// Utility method for applying an upward force and forward force to a ball at a specific index.
    /// </summary>
    /// <param name="upwardForce">Upward force to be applied to the ball at the specified index.</param>
    /// <param name="forwardForce">Forward force to be applied to the ball at the specified index.</param>
    /// <param name="ballIndex">Index of the ball which will receive the applied force.</param>
    public void ApplyForcesToSpecificBall(float upwardForce, float forwardForce, int ballIndex)
    {
        BallController specificBall;
        if (TryGetBallAtSpecificIndex(ballIndex, out specificBall))
            specificBall.ApplyForce(upwardForce, forwardForce);
    }

    /// <summary>
    /// Utility method for acquiring a ball at a specified index.
    /// </summary>
    /// <param name="ballIndex">Index for acquiring a specific ball.</param>
    /// <param name="ballController">BallController which is referenced if ball at specified index is found.</param>
    /// <returns>True if ball is found at specified index. False if otherwise.</returns>
    public bool TryGetBallAtSpecificIndex(int ballIndex, out BallController ballController)
    {
        if (SpawnedBallControllers != null)
        {
            if (ballIndex < SpawnedBallControllers.Length)
            {
                ballController = SpawnedBallControllers[ballIndex];
                return true;
            }
        }

        ballController = null;
        return false;
    }

    /// <summary>
    /// Utility method for resetting all balls associated with the player.
    /// </summary>
    public void ResetBalls()
    {
        foreach (BallController ballController in SpawnedBallControllers)
            ballController.Reset();
    }
}
