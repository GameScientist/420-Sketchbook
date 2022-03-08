using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : MonoBehaviour
{
    private float forceSeparation = 1f;
    private float forceCohesion = .25f;
    private float forceAlignment = .75f;
    private float forceAvoid = 2;
    private float forceAttract = 1;
    private float speed;
    private float perception;
    private BoidManager manager;
    private Rigidbody body;
    public Vector3 direction = new();
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        manager = BoidManager.singleton;
        speed = Random.Range(1f, 2f);
        perception = Random.Range(0f, 1f);
        manager.preys.Add(this);
        body.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 force = GroupMovement();
        force = NestMovement(10 * perception, manager.preyNests, force, transform.position, forceAttract);
        force = NestMovement(3 * perception, manager.predatorNests, force, Vector3.zero, forceAvoid);
        if (force.magnitude > speed) force = force.normalized * speed;
        body.AddForce(force * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(body.velocity);
    }

    private Vector3 AvoidMovement(Vector3 force)
    {
        Vector3 avoider = new Vector3();
        float avoidDistance = 3;
        foreach (Nest predatorNest in manager.predatorNests)
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

    private Vector3 AttractionMovement(Vector3 force)
    {
        Vector3 attractor = new Vector3();
        float attractionDistance = 10;
        foreach (Nest preyNest in manager.preyNests)
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

    private Vector3 NestMovement(float maxDistance, List<Nest> nests, Vector3 force, Vector3 positionModifier, float forceModifier)
    {
        Vector3 objectPosition = new Vector3();
        float distance = maxDistance;
        foreach(Nest nest in nests)
        {
            float potentialDistance = Vector3.Distance(transform.position, nest.transform.position);
            if(potentialDistance < distance)
            {
                if (nest.predator) objectPosition = (transform.position - nest.transform.position).normalized;
                else objectPosition = nest.transform.position;
                distance = potentialDistance;
            }
        }
        if (distance < maxDistance) force += (objectPosition - positionModifier).normalized * forceModifier;
        return force;
    }

    private Vector3 GroupMovement()
    {
        Vector3 force = new Vector3(), groupCenter = new Vector3(), groupAlignment = new Vector3(), groupSeperation = new Vector3();
        int cohesiveBoids = 0, alignedBoids = 0, separatedBoids = 0;
        foreach (Prey prey in manager.preys)
        {
            if (prey == this) continue;
            float distance = Vector3.Distance(transform.position, prey.transform.position);
            if (distance < 4 * perception)
            {
                groupCenter += prey.transform.position / distance;
                cohesiveBoids++;
            }
            if (distance < 2 * perception)
            {
                groupAlignment += prey.body.velocity / distance;
                alignedBoids++;
            }
            if (distance < .5f * perception)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Predator>()) Destroy(gameObject);
    }

    private void OnDestroy() => manager.preys.Remove(this);
}

