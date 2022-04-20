using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private RocketManager manager;
    private Rigidbody body;
    private float checkTimer = 0;
    private bool check;
    private List<Pathfinding.Node> pathToWaypoint = new List<Pathfinding.Node>();
    // Start is called before the first frame update
    void Start()
    {
        manager = RocketManager.singleton;
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
            if (GridController2D.singleton) LookupPath();
        }
        ApplyForce();
        transform.rotation = Quaternion.LookRotation(body.velocity);
    }

    private void ApplyForce()
    {
        Vector3 force = DirectionToWaypoint(), groupCenter = new Vector3(), groupAlignment = new Vector3(), groupSeperation = new Vector3();
        int cohesiveBoids = 0, alignedBoids = 0, separatedBoids = 0;
        foreach (Rocket rocket in manager.rockets)
        {
            if (rocket == this) continue;
            float distance = Vector3.Distance(transform.position, rocket.transform.position);
            if (distance < 8) // Move the prey into the center of the school.
            {
                groupCenter += rocket.transform.position / distance;
                cohesiveBoids++;
            }
            if (distance < 4)// Move the prey in the same direction as nearby fish.
            {
                groupAlignment += rocket.body.velocity / distance;
                alignedBoids++;
            }
            if (distance < 2)// Move the prey so that avoids collision with other fish.
            {
                groupSeperation += (transform.position - rocket.transform.position).normalized / distance;
                separatedBoids++;
            }
        }
        if (cohesiveBoids > 0) force += (groupCenter / cohesiveBoids - transform.position).normalized * 0.1f;
        if (alignedBoids > 0) force += (groupAlignment / alignedBoids - body.velocity).normalized * 0.3f;
        if (separatedBoids > 0) force += (groupSeperation / separatedBoids).normalized * 0.2f;
        body.AddForce(force.normalized * 64 * Time.deltaTime);
    }

    private Vector3 DirectionToWaypoint()
    {
        Vector3 direction = pathToWaypoint[1].pos - transform.position;
        if (direction.magnitude < .1f) check = true;
        Vector3 force = direction.normalized * .4f;
        return force;
    }

    private void LookupPath()
    {
        Pathfinding.Node start = GridController2D.singleton.Lookup(transform.position);
        Pathfinding.Node end = GridController2D.singleton.Lookup(manager.transform.position);
        if (start == null || end == null || start == end) pathToWaypoint.Clear();
        else pathToWaypoint = Pathfinding.Solve(start, end);
    }
}
