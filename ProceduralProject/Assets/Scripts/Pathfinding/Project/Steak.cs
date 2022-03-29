using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steak : MonoBehaviour
{
    public Transform waypoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PathfinderController player = collision.GetComponent<PathfinderController>();
        if (player != null) player.waypoint = waypoint;
        Destroy(gameObject);
    }
}
