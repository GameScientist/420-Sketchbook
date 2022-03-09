using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : MonoBehaviour
{
    /// <summary>
    /// Used to weigh how much of each type of force should be applied to the prey.
    /// </summary>
    private float forceAlignment = .575f, forceAttract = 3, forceAvoid = 2, forceCohesion = .125f, forceSeparation = .5f;
    /// <summary>
    /// Used to decide on how the prey will behave next.
    /// </summary>
    private float fullness, perception, speed;
    /// <summary>
    /// Referenced to adjust the prey list.
    /// </summary>
    private BoidManager manager;
    /// <summary>
    /// Has force applied to it.
    /// </summary>
    private Rigidbody body;
    /// <summary>
    /// The current direction of the prey.
    /// </summary>
    public Vector3 direction = new();
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        fullness = Random.Range(10f, 100f);
        manager = BoidManager.singleton;
        perception = Random.Range(0f, 1f);
        speed = Random.Range(1f, 2f);
        manager.preys.Add(this);
        body.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed);
    }

    // Update is called once per frame
    void Update()
    {
        fullness -= Time.deltaTime;
        if (fullness <= 0)// If the prey goes to long without eatting, it starves to death.
        {
            body.useGravity = true;
            if (fullness <= -5f)// After being dead long enough, the prey despawns.
            {
                manager.preys.Remove(this);
                Destroy(gameObject);
            }
            return;
        }
        Vector3 force = GroupMovement();
        force = AttractionMovement(force);
        force = AvoidMovement(force);
        if (force.magnitude > speed) force = force.normalized * speed;
        body.AddForce(force * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(body.velocity);
    }

    /// <summary>
    /// Finds the force used to navigate the prey within a school of fish.
    /// </summary>
    /// <returns>The total force of the prey.</returns>
    private Vector3 GroupMovement()
    {
        Vector3 force = new Vector3(), groupCenter = new Vector3(), groupAlignment = new Vector3(), groupSeperation = new Vector3();
        int cohesiveBoids = 0, alignedBoids = 0, separatedBoids = 0;
        foreach (Prey prey in manager.preys) // Find out where to move in relation to another fish.
        {
            if (prey == this) continue;
            float distance = Vector3.Distance(transform.position, prey.transform.position);
            if (distance < 4 * perception) // Move the prey into the center of the school.
            {
                groupCenter += prey.transform.position / distance;
                cohesiveBoids++;
            }
            if (distance < 2 * perception)// Move the prey in the same direction as nearby fish.
            {
                groupAlignment += prey.body.velocity / distance;
                alignedBoids++;
            }
            if (distance < .5f * perception)// Move the prey so that avoids collision with other fish.
            {
                groupSeperation += (transform.position - prey.transform.position).normalized / distance;
                separatedBoids++;
            }
        }
        if (cohesiveBoids > 0) force += (groupCenter / cohesiveBoids - transform.position).normalized * forceCohesion;
        if (alignedBoids > 0) force += (groupAlignment / alignedBoids - body.velocity).normalized * forceAlignment;
        if (separatedBoids > 0) force += (groupSeperation / separatedBoids).normalized * forceSeparation;
        return force;
    }

    /// <summary>
    /// Leads the fish towards objects that attract it.
    /// </summary>
    /// <param name="force">The old force of the object.</param>
    /// <returns>The new force of the object.</returns>
    private Vector3 AttractionMovement(Vector3 force)
    {
        Vector3 attractor = new Vector3();
        float attractionDistance = 10;
        if (manager.poop.Count > 0) foreach (GameObject poop in manager.poop) // Check for the nearest poop.
            {
                float potentialDistance = Vector3.Distance(transform.position, poop.transform.position);
                if (potentialDistance < attractionDistance)
                {
                    attractor = poop.transform.position;
                    attractionDistance = potentialDistance;
                }
            }
        else if (manager.preyNests.Count > 0) foreach (Nest preyNest in manager.preyNests) // If there is no poop, check for the nearest nest.
            {
                float potentialDistance = Vector3.Distance(transform.position, preyNest.transform.position);
                if (potentialDistance < attractionDistance)
                {
                    attractor = preyNest.transform.position;
                    attractionDistance = potentialDistance;
                }
            }
        if (attractionDistance < 10) force += (attractor - transform.position).normalized * forceAttract;
        return force;
    }

    /// <summary>
    /// Leads the fish away from objects it sees as a threat.
    /// </summary>
    /// <param name="force">The old force of the prey.</param>
    /// <returns>The new force of the prey.</returns>
    private Vector3 AvoidMovement(Vector3 force)
    {
        Vector3 avoider = new Vector3();
        float avoidDistance = 3;
        // Check for the closes
        if (manager.predators.Count > 0) foreach (Predator predator in manager.predators)// Check for the nearest predator.
            {
                float potentialDistance = Vector3.Distance(transform.position, predator.transform.position);
                if (potentialDistance < avoidDistance)
                {
                    avoider = (transform.position - predator.transform.position).normalized;
                    avoidDistance = potentialDistance;
                }
            }
        if (manager.predatorNests.Count > 0) foreach (Nest predatorNest in manager.predatorNests)// If there are no predators, check for the nearest predator nest.
            {
                float potentialDistance = Vector3.Distance(transform.position, predatorNest.transform.position);
                if (potentialDistance < avoidDistance)
                {
                    avoider = (transform.position - predatorNest.transform.position).normalized;
                    avoidDistance = potentialDistance;
                }
            }
        if (avoidDistance < 3) force += avoider.normalized * forceAvoid;
        return force;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Poop")) // The prey eats poop to keep itself from starving. That's a nice thought, now isn't it?
        {
            manager.poop.Remove(collision.gameObject);
            Destroy(collision.gameObject);
            fullness += Random.Range(1f, 10f);
        }
        if (collision.gameObject.GetComponent<Predator>()) Destroy(gameObject);
    }

    private void OnDestroy() => manager.preys.Remove(this);
}

