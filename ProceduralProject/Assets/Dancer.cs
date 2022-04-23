using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves with other dancers to a white panel in the scene.
/// </summary>
public class Dancer : MonoBehaviour
{
    /// <summary>
    /// Signals that it is time to check for a new path to take to the destination.
    /// </summary>
    private bool check;
    /// <summary>
    /// How long before another check is made.
    /// </summary>
    private float checkTimer = 0;
    /// <summary>
    /// Holds a list of every dancer.
    /// </summary>
    private LightShow manager;
    /// <summary>
    /// The current path that is planned to be take to the waypoint.
    /// </summary>
    private List<Pathfinding.Node> pathToWaypoint = new List<Pathfinding.Node>();
    /// <summary>
    /// Force is applied to this help the dancer travel to the white panel.
    /// </summary>
    private Rigidbody body;
    /// <summary>
    /// The location of the white panel that the dancer is traveling to.
    /// </summary>
    public Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        manager = LightShow.singleton;
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0)// Reset check timer.
        {
            check = true;
            checkTimer = .25f;
        }
        if (check)
        {
            check = false;
            if (DanceFloor.singleton) LookupPath();
        }
        ApplyForce();
        if (body.velocity.magnitude < .1f) Quaternion.LookRotation(Vector3.up * 180);
        else transform.rotation = Quaternion.LookRotation(new Vector3(body.velocity.x, 0, body.velocity.z));
    }

    /// <summary>
    /// Adds force to the dancer in the direction of its destination while also being altered by nearby dancers.
    /// </summary>
    private void ApplyForce()
    {
        // The different forceds being applied to the dancer.
        Vector3 force = Vector3.zero, groupCenter = new Vector3(), groupSeperation = new Vector3();
        // The number of nearby dancers.
        int cohesiveBoids = 0, separatedBoids = 0;
        if (pathToWaypoint.Count > 0 && Vector3.Distance(transform.position - Vector3.up, destination) > 1) force = DirectionToWaypoint();
        foreach (Dancer dancer in manager.dancers)
        {
            if (dancer == this) continue;
            float distance = Vector3.Distance(transform.position, dancer.transform.position);
            if (distance < 8)
            {
                groupCenter += dancer.transform.position / distance;
                cohesiveBoids++;
            }
            if (distance < 4)// Move the prey so that avoids collision with other fish.
            {
                groupSeperation += (transform.position - dancer.transform.position).normalized / distance;
                separatedBoids++;
            }
        }
        if (cohesiveBoids > 0)
        {
            Vector3 cohesion = groupCenter / cohesiveBoids - transform.position;
            force += new Vector3(cohesion.x, 0, cohesion.z).normalized * 0.25f;
        }
        if (separatedBoids > 0)
        {
            Vector3 separation = groupSeperation / separatedBoids;
            force += new Vector3(separation.x, 0, separation.z).normalized * 0.75f;
        }
        force = new Vector3(force.x, 0, force.z);
        body.AddForce(force.normalized);
    }

    /// <returns>The direction the dancer must take to its next waypoint.</returns>
    private Vector3 DirectionToWaypoint()
    {
        Vector3 direction = pathToWaypoint[1].pos - transform.position;
        if (direction.magnitude < 1) check = true;
        Vector3 force = new Vector3(direction.x, 0, direction.z).normalized;
        return force;
    }

    /// <summary>
    /// Finds the shortest path to the white panel, if any.
    /// </summary>
    private void LookupPath()
    {
        Pathfinding.Node start = DanceFloor.singleton.Lookup(transform.position - DanceFloor.singleton.transform.position);
        Pathfinding.Node end = DanceFloor.singleton.Lookup(destination - DanceFloor.singleton.transform.position);
        if (start == null || end == null || start == end) pathToWaypoint.Clear();
        else pathToWaypoint = Pathfinding.Solve(start, end);
    }
}
