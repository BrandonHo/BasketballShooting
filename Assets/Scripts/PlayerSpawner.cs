using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Vector3 SpawnAreaDimensions;
    public GameObject PlayerPrefab;
    private GameObject SpawnedPlayer;
    private PlayerController SpawnedPlayerController;

    public void SpawnPlayer()
    {
        if (PlayerPrefab)
        {
            // Generate a random position within specified spawn area
            Vector3 spawnedPosition = new Vector3(Random.Range(0f, SpawnAreaDimensions.x / 2f), 0f, Random.Range(0f, SpawnAreaDimensions.z / 2f));
            spawnedPosition += transform.position;
            spawnedPosition += new Vector3(0f, PlayerPrefab.transform.localScale.y, 0f);

            // Instantiate player using random position
            SpawnedPlayer = Instantiate(PlayerPrefab, spawnedPosition, transform.rotation, transform);
            SpawnedPlayerController = SpawnedPlayer.GetComponent<PlayerController>();
        }
    }

    public void SpawnBalls(int numberOfBalls, Transform target)
    {
        if (SpawnedPlayerController)
            SpawnedPlayerController.SpawnBalls(numberOfBalls, target);
    }

    public void ApplyForceToSpecificBall(float upwardForce, float forwardForce, int ballIndex)
    {
        if (SpawnedPlayerController)
            SpawnedPlayerController.ApplyForceToSpecificBall(upwardForce, forwardForce, ballIndex);
    }

    public void SetPlayerLookTarget(Transform targetTransform)
    {
        if (SpawnedPlayer)
            SpawnedPlayer.transform.LookAt(new Vector3(targetTransform.position.x, SpawnedPlayer.transform.position.y, targetTransform.position.z));
    }

    public bool TryGetBallAtSpecificIndex(int ballIndex, out BallController ballController)
    {
        if (SpawnedPlayerController)
        {
            SpawnedPlayerController.TryGetBallAtSpecificIndex(ballIndex, out ballController);
            return true;
        }

        ballController = null;
        return false;
    }

    public void ResetPlayerBalls()
    {
        if (SpawnedPlayerController)
            SpawnedPlayerController.ResetBalls();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, SpawnAreaDimensions);
    }
}
