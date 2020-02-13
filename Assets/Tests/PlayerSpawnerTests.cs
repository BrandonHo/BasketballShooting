using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlayerSpawnerTests
{
    private PlayerSpawner PlayerSpawnerComponent;

    [SetUp]
    public void Setup()
    {
        GameObject spawnerGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/PlayerSpawner"));
        PlayerSpawnerComponent = spawnerGameObject.GetComponent<PlayerSpawner>();
    }

    [TearDown]
    public void Teardown()
    {
        if (PlayerSpawnerComponent)
            Object.Destroy(PlayerSpawnerComponent.gameObject);
    }

    [UnityTest]
    public IEnumerator SpawnPlayerTest()
    {
        // Spawn player
        PlayerSpawnerComponent.SpawnPlayer();
        yield return new WaitForSeconds(0.1f);

        // Assert status based on if player actually spawned
        Assert.IsTrue(PlayerSpawnerComponent.IsPlayerSpawned());
        PlayerSpawnerComponent.DestroySpawnedPlayer();
    }

    [UnityTest]
    public IEnumerator SpawnBallsTest()
    {
        // Spawn player and balls
        PlayerSpawnerComponent.SpawnPlayer();
        yield return new WaitForSeconds(0.1f);
        PlayerSpawnerComponent.SpawnBalls(10, PlayerSpawnerComponent.transform);

        // Assert status based on if can acquire the last ball (9 since list starts from zero)
        BallController aBallController;
        PlayerSpawnerComponent.TryGetBallAtSpecificIndex(9, out aBallController);
        Assert.IsTrue(aBallController);

        // Garbage collection
        PlayerSpawnerComponent.DestroySpawnedPlayer();
    }

    [UnityTest]
    public IEnumerator ApplyForcesToBallTest()
    {
        // Spawn player and balls
        PlayerSpawnerComponent.SpawnPlayer();
        yield return new WaitForSeconds(0.1f);
        PlayerSpawnerComponent.SpawnBalls(10, PlayerSpawnerComponent.transform);

        // Get last ball, reference initial position
        BallController aBallController;
        PlayerSpawnerComponent.TryGetBallAtSpecificIndex(9, out aBallController);
        float initialYPosition = aBallController.transform.position.y;

        // Apply force to last ball
        PlayerSpawnerComponent.ApplyForceToSpecificBall(30f, 30f, 9);
        yield return new WaitForSeconds(0.1f);

        // Assert status based on if ball moved
        Assert.AreNotEqual(initialYPosition, aBallController.transform.position.y);

        // Garbage collection
        PlayerSpawnerComponent.DestroySpawnedPlayer();
    }
}
