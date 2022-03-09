using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Eats prey and poops.
/// No, really, this fish poops.
/// </summary>
public class Predator : MonoBehaviour
{
    /// <summary>
    /// An attribute of this predator used to decide on its behavior.
    /// </summary>
    private float fullness, poopTimer, poopBurstTimer = 0, speed;
    /// <summary>
    /// Referenced to adjust the list of predators.
    /// </summary>
    private BoidManager manager;
    /// <summary>
    /// Spawned when the fish poops.
    /// </summary>
    [SerializeField]
    private GameObject poopPrefab;
    /// <summary>
    /// Has force applied to it to accelerate it into a direction.
    /// </summary>
    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        fullness = Random.Range(10f, 100f);
        manager = BoidManager.singleton;
        poopTimer = Random.Range(1f, 6f);
        speed = Random.Range(1f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        fullness -= Time.deltaTime;
        if (fullness <= 0)// If the predator goes for too long without food, it starves to death.
        {
            body.useGravity = true;
            if (fullness <= -5f)// After being dead for long enough, the predator despawns.
            {
                manager.predators.Remove(this);
                Destroy(gameObject);
            }
            return;
        }
        Move();
        transform.rotation = Quaternion.LookRotation(body.velocity);
        if (poopTimer <= 0) // The predator poops in bursts after a delay.
        {
            poopBurstTimer -= Time.deltaTime;
            if (poopBurstTimer <= 0)
            {
                manager.poop.Add(Instantiate(poopPrefab, transform.position - (transform.up / 2), transform.rotation, null));
                if (Random.Range(0f, 1f) < .1f) poopTimer = Random.Range(1f, 6f);
                else poopBurstTimer = Random.Range(.1f, 1);
            }
        }
        else poopTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Propels the fish into the direction of a prey or nest.
    /// </summary>
    private void Move()
    {
        Vector3 force = new Vector3();
        float distance = 10;
        if (manager.preys.Count > 0) foreach (Prey prey in manager.preys)
            {
                float potentialDistance = Vector3.Distance(transform.position, prey.transform.position);
                if (potentialDistance < distance)
                {
                    force = (prey.transform.position - transform.position).normalized * speed * Time.deltaTime;
                    distance = potentialDistance;
                }
            }
        else if (manager.predatorNests.Count > 0) foreach (Nest nest in manager.predatorNests)
            {
                float potentialDistance = Vector3.Distance(transform.position, nest.transform.position);
                if (potentialDistance < distance)
                {
                    force = (nest.transform.position - transform.position).normalized * speed * Time.deltaTime;
                    distance = potentialDistance;
                }
            }
        body.AddForce(force);
    }

    /// <summary>
    /// Predators eat prey to prevent starvation.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (fullness <= 0) return;
        Prey prey = collision.gameObject.GetComponent<Prey>();
        if (prey != null) fullness += Random.Range(1f, 10f);
    }
}
