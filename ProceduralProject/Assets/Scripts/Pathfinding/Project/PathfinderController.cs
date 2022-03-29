using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderController : MonoBehaviour
{
    private bool check;
    private float checkTimer = 0;
    private List<Pathfinding.Node> pathToWaypoint = new List<Pathfinding.Node>();
    [SerializeField]
    private Transform waypoint;
    private LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0)
        {
            check = true;
            checkTimer = 1;
        }
        if (check) FindPath();
        if (pathToWaypoint != null && pathToWaypoint.Count >= 2) MoveAlongPath();
    }

    private void FindPath()
    {
        check = false;
        if (waypoint && GridController2D.singleton)
        {
            Pathfinding.Node start = GridController2D.singleton.Lookup(transform.position);
            Pathfinding.Node end = GridController2D.singleton.Lookup(waypoint.position);
            print(start.pos);
            if (start == null || end == null || start == end)
            {
                pathToWaypoint.Clear();
                return;
            }

            pathToWaypoint = Pathfinding.Solve(start, end);

            // Set up rendering the path for line renderer
            Vector3[] positions = new Vector3[pathToWaypoint.Count];

            for (int i = 0; i < pathToWaypoint.Count; i++) positions[i] = pathToWaypoint[i].pos - new Vector3(0, 0, .5f);
            line.positionCount = positions.Length;
            line.SetPositions(positions);
        }
    }

    private void MoveAlongPath()
    {
        Vector3 destination = pathToWaypoint[1].pos;
        transform.position = Vector3.Lerp(transform.position, destination, .01f);
        float d = (destination - transform.position).magnitude;
        if (d < .25f) check = true;
    }
}
