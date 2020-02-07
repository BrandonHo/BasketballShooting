using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject[] SpawnedBalls;
    private BallController[] SpawnedBallControllers;
    public GameObject BallPrefab;
    public Transform BallSpawnerTransform;

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

    public void ApplyForceToSpecificBall(float upwardForce, float forwardForce, int ballIndex)
    {
        BallController specificBall;
        if (TryGetBallAtSpecificIndex(ballIndex, out specificBall))
            specificBall.ApplyForce(upwardForce, forwardForce);
    }

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

    public void ResetBalls()
    {
        foreach (BallController ballController in SpawnedBallControllers)
            ballController.Reset();
    }
}
