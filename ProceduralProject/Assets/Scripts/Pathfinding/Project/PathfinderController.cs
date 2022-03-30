using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderController : MonoBehaviour
{
    private bool check;
    private float checkTimer = 0;
    private List<Pathfinding.Node> pathToWaypoint = new List<Pathfinding.Node>();
    public Transform waypoint;
    private LineRenderer line;
    private Rigidbody2D body;
    public bool moving;
    [SerializeField]
    private GameObject rightTutorial;
    [SerializeField]
    private GameObject leftTutorial;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) moving = true;
        if (Input.GetMouseButtonDown(1) && rightTutorial.activeInHierarchy)
        {
            rightTutorial.SetActive(false);
            leftTutorial.SetActive(true);
        }
        if (Input.GetMouseButtonDown(0) && leftTutorial.activeInHierarchy) leftTutorial.SetActive(false);
        if (!moving) return;
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
        //transform.position = Vector3.Lerp(transform.position, destination, .01f);
        Vector2 direction = destination - transform.position;
        body.velocity = Vector3.Lerp(body.velocity, direction.normalized, .02f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90, Vector3.forward), .02f);
        float d = direction.magnitude;
        if (d < .25f) check = true;
    }
}
