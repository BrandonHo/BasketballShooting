using UnityEngine;

/// <summary>
/// Component which controls the behaviour of the player spawner game object.
/// </summary>
public class PlayerSpawner : MonoBehaviour
{
    public Vector3 SpawnAreaDimensions;                 // Area dimensions which indicates where player can be spawned.
    public GameObject PlayerPrefab;                     // Prefab used for instantiating the player game object.
    private GameObject SpawnedPlayer;                   // Reference to the spawned player game object.
    private PlayerController SpawnedPlayerController;   // Reference to the player controller of the spawned player game object.

    /// <summary>
    /// Utility method for spawning a player game object within the spawn area.
    /// </summary>
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

    /// <summary>
    /// Utility method for spawning the balls associated with the player game object.
    /// </summary>
    /// <param name="numberOfBalls">The number of balls to be spawned.</param>
    /// <param name="target">Target transform which is used for ball-related calculations.</param>
    public void SpawnBalls(int numberOfBalls, Transform target)
    {
        if (SpawnedPlayerController)
            SpawnedPlayerController.SpawnBalls(numberOfBalls, target);
    }

    /// <summary>
    /// Utility method for applying forces to a ball, associated with the player game object, at a specific index.
    /// </summary>
    /// <param name="upwardForce">Upward force to be applied to the ball at specified index.</param>
    /// <param name="forwardForce">Forward force to be applied to the ball at specified index.</param>
    /// <param name="ballIndex">Index of specific ball to receive the applied forces.</param>
    public void ApplyForceToSpecificBall(float upwardForce, float forwardForce, int ballIndex)
    {
        if (SpawnedPlayerController)
            SpawnedPlayerController.ApplyForcesToSpecificBall(upwardForce, forwardForce, ballIndex);
    }

    /// <summary>
    /// Utility method for setting the look direction of the player game object.
    /// </summary>
    /// <param name="targetTransform">Target transform to be looked at by the player game object.</param>
    public void SetPlayerLookTarget(Transform targetTransform)
    {
        if (SpawnedPlayer)
            SpawnedPlayer.transform.LookAt(new Vector3(targetTransform.position.x, SpawnedPlayer.transform.position.y, targetTransform.position.z));
    }

    /// <summary>
    /// Utility method for acquiring a ball at a specified index.
    /// </summary>
    /// <param name="ballIndex">Index for acquiring specific ball.</param>
    /// <param name="ballController">BallController which is referenced if ball at specified index exists.</param>
    /// <returns>True if ball is found at specified index. False if otherwise.</returns>
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

    /// <summary>
    /// Helper method for resetting the balls associated with the player game object.
    /// </summary>
    public void ResetPlayerBalls()
    {
        if (SpawnedPlayerController)
            SpawnedPlayerController.ResetBalls();
    }

    /// <summary>
    /// Utility debug method used for unit tests. Returns a boolean based on
    /// if the player game object has been instantiated.
    /// </summary>
    /// <returns>True if player game object has been instantiated. False if otherwise.</returns>
    public bool IsPlayerSpawned()
    {
        if (SpawnedPlayer)
            return true;
        return false;
    }

    public void DestroySpawnedPlayer()
    {
        if (SpawnedPlayer)
            Destroy(SpawnedPlayer);
    }

    /// <summary>
    /// Debug method used for displaying the spawn area dimensions in the editor.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, SpawnAreaDimensions);
    }
}
