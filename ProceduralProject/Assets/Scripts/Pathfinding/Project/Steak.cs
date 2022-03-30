using UnityEngine;

/// <summary>
/// Sets a new waypoint for the player upon collision.
/// </summary>
public class Steak : MonoBehaviour
{
    /// <summary>
    /// The next waypoint of the player.
    /// </summary>
    public Transform waypoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PathfinderController player = collision.GetComponent<PathfinderController>();
        if (player != null) player.waypoint = waypoint;
        Destroy(gameObject);
    }
}
