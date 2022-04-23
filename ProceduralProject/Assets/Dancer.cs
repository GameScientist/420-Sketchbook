using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dancer : MonoBehaviour
{
    private LightShow manager;
    private Rigidbody body;
    private float checkTimer = 0;
    private bool check;
    private List<Pathfinding.Node> pathToWaypoint = new List<Pathfinding.Node>();
    public Vector3 destination = new Vector3(8, 4);
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

    private void ApplyForce()
    {
        Vector3 force = Vector3.zero, groupCenter = new Vector3(), groupSeperation = new Vector3();
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
        print("Cohesiveness: " + (groupCenter / cohesiveBoids - transform.position).normalized * 0.25f);
        print("Separation: " + (groupSeperation / separatedBoids).normalized * 0.75f);
        if (cohesiveBoids > 0)
        {
            Vector3 cohesion = groupCenter / cohesiveBoids - transform.position;
            force += new Vector3(cohesion.x, 0, cohesion.z).normalized * 0.25f;
            Debug.DrawRay(transform.position, new Vector3(cohesion.x, 0, cohesion.z).normalized * 0.25f, Color.blue);
        }
        if (separatedBoids > 0)
        {
            Vector3 separation = groupSeperation / separatedBoids;
            force += new Vector3(separation.x, 0, separation.z).normalized * 0.75f;
            Debug.DrawRay(transform.position, new Vector3(separation.x, 0, separation.z).normalized * 0.75f, Color.red);
        }
        force = new Vector3(force.x, 0, force.z);
        body.AddForce(force.normalized);
        Debug.DrawRay(transform.position, force, Color.white);
        //if (body.velocity.magnitude > 1) body.velocity.Normalize();
    }

    private Vector3 DirectionToWaypoint()
    {
        Vector3 direction = pathToWaypoint[1].pos - transform.position;
        if (direction.magnitude < 1) check = true;
        Vector3 force = new Vector3(direction.x, 0, direction.z).normalized;
        Debug.DrawRay(transform.position, force, Color.green);
        return force;
    }

    private void LookupPath()
    {
        Pathfinding.Node start = DanceFloor.singleton.Lookup(transform.position - DanceFloor.singleton.transform.position);
        Pathfinding.Node end = DanceFloor.singleton.Lookup(destination - DanceFloor.singleton.transform.position);
        if (start == null || end == null || start == end) pathToWaypoint.Clear();
        else pathToWaypoint = Pathfinding.Solve(start, end);
    }
}
