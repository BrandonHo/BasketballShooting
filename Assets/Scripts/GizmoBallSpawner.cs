using UnityEngine;

/// <summary>
/// Component which is purely used for debugging purposes. This component
/// was designed to illustrate the spawning position of balls from the player.
/// </summary>
public class GizmoBallSpawner : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
