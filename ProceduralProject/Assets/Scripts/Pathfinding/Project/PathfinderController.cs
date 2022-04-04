using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Follows a path to reach waypoints.
/// </summary>
public class PathfinderController : MonoBehaviour
{
    /// <summary>
    /// Signals that the controller can search for a new path.
    /// </summary>
    private bool check;
    /// <summary>
    /// How long before the controller automatically checks for a new path.
    /// </summary>
    private float checkTimer = 0;
    /// <summary>
    /// The tutorial explaining how to place a right floor.
    /// </summary>
    [SerializeField]
    private GameObject rightTutorial;
    /// <summary>
    /// The tutorial explaining how to place a left floor.
    /// </summary>
    [SerializeField]
    private GameObject leftTutorial;
    /// <summary>
    /// A message that is shown when the player character reaches the goal.
    /// </summary>
    [SerializeField]
    private GameObject victoryScreen;
    /// <summary>
    /// The currently selected path to a waypoint.
    /// </summary>
    private List<Pathfinding.Node> pathToWaypoint = new List<Pathfinding.Node>();
    /// <summary>
    /// Moves the player character.
    /// </summary>
    private Rigidbody2D body;

    /// <summary>
    /// Signals that the player controller can start moving towards its destination.
    /// </summary>
    public bool moving;
    /// <summary>
    /// The current point the player is attempting to move to.
    /// </summary>
    public Transform waypoint;

    // Start is called before the first frame update
    void Start() => body = GetComponent<Rigidbody2D>();

    // Update is called once per frame
    void Update()
    {
        // Tutorials and movement are enabled by input.
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) moving = true;
        if (Input.GetMouseButtonDown(1) && rightTutorial.activeInHierarchy)
        {
            rightTutorial.SetActive(false);
            leftTutorial.SetActive(true);
        }
        if (Input.GetMouseButtonDown(0) && leftTutorial.activeInHierarchy) leftTutorial.SetActive(false);
        if (!moving) return;

        if(Vector3.Distance(transform.position, new Vector3(10, 5, 0)) < .5f)
        {
            victoryScreen.SetActive(true);
            Destroy(gameObject);
        }

        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0)// Reset check timer.
        {
            check = true;
            checkTimer = .25f;
        }
        if (check) FindPath();
        if (pathToWaypoint != null && pathToWaypoint.Count >= 2) MoveAlongPath();
    }

    /// <summary>
    /// Creates a new path for the player character to take towards the current waypoint.
    /// </summary>
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
        }
    }

    /// <summary>
    /// Moves the player character in the direction of the path.
    /// </summary>
    private void MoveAlongPath()
    {
        Vector2 direction = pathToWaypoint[1].pos - transform.position;
        body.velocity = Vector3.Lerp(body.velocity, direction.normalized, .02f);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90, Vector3.forward), .02f);
        float d = direction.magnitude;
        if (d < .25f) check = true;
    }
}
