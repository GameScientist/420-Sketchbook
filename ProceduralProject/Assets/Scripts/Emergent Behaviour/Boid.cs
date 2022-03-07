using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private float avoidForce;
    private float separationForce;
    private float speed;
    private float steering;
    private float acceleration;
    private Rigidbody body;
    public Vector3 currentVelocity;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        separationForce = Random.Range(.1f, .5f);
        avoidForce = Random.Range(.1f, 7);
        speed = Random.Range(0.1f, 4f);
        acceleration = speed * Random.Range(.1f, 1);
        steering = Random.Range(.1f, .9f);
        transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        body.velocity = transform.forward;
        currentVelocity = body.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity = body.velocity;
        Vector3 seperationDestination = new Vector3(), alignmentDestination = new Vector3(), cohesionDestination = new Vector3();
        int separationBoids = 0, alignmentBoids = 0, cohesionBoids = 0;
        bool nearbyDestination = false;
        foreach (Boid boid in transform.parent.GetComponentsInChildren<Boid>())
        {
            if (boid == this) continue;
            float distance = Vector3.Distance(boid.transform.position, transform.position);
            if (distance < 1.4)
            {
                //Debug.DrawLine(transform.position, transform.position + transform.position - boid.transform.position, Color.red);
                seperationDestination += Vector3.Lerp(transform.position - (boid.transform.position - transform.position), transform.position, distance / 1.4f);
                //seperationDestination += transform.position - ((boid.transform.position - transform.position) * (separationForce/distance));
                separationBoids++;
                nearbyDestination = true;
            }
            //print(body.velocity);
            if (distance < 2.8)
            {
                //Debug.DrawLine(transform.position, boid.currentVelocity.normalized + transform.position, Color.green);
                //alignmentDestination += boid.currentVelocity.normalized + transform.position;
                alignmentDestination += Vector3.Lerp(boid.currentVelocity.normalized + transform.position, transform.position, distance / 2.8f);
                alignmentBoids++;
                nearbyDestination = true;
            }
            if (distance < 5.6)
            {
                //cohesionDestination += boid.transform.position;
                cohesionDestination += Vector3.Lerp(boid.transform.position, transform.position, distance / 5.6f);
                cohesionBoids++;
                nearbyDestination = true;
            }
        }
        bool nearbyAvoider = false;
        Vector3 avoidDestination = new Vector3();
        foreach (GameObject avoider in GameObject.FindGameObjectsWithTag("Avoider"))
        {
            float distance = Vector3.Distance(avoider.transform.position, transform.position);
            if (distance < 4.2 && (distance < Vector3.Distance(avoidDestination, transform.position) || !nearbyAvoider))
            {
                avoidDestination = transform.position - ((avoidDestination - transform.position) * (avoidForce / distance));
                avoidDestination = Vector3.Lerp(transform.position - (avoidDestination - transform.position), transform.position, distance / 4.2f);
                if (!nearbyAvoider) nearbyAvoider = true;
                nearbyDestination = true;
            }
        }
        Vector3 attractDestination = new Vector3();
        bool nearbyAttractor = false;
        foreach (GameObject attractor in GameObject.FindGameObjectsWithTag("Attractor"))
        {
            if (Vector3.Distance(attractor.transform.position, transform.position) < Vector3.Distance(attractDestination, transform.position) || !nearbyAttractor)
            {
                attractDestination = attractor.transform.position;
                if (!nearbyAttractor) nearbyAttractor = true;
                nearbyDestination = true;
            }
        }
        if (nearbyDestination)
        {
            Vector3 finalDestination = new Vector3();
            int subdestinations = 0;
            if (separationBoids > 0)
            {
                seperationDestination /= separationBoids;
                Debug.DrawLine(transform.position, seperationDestination, Color.yellow);
                finalDestination += seperationDestination / 2;
                subdestinations++;
            }
            if (alignmentBoids > 0)
            {
                //Debug.DrawLine(transform.position, alignmentDestination, Color.green);
                alignmentDestination /= alignmentBoids;
                Debug.DrawLine(transform.position, alignmentDestination, Color.magenta);
                finalDestination += alignmentDestination * .375f;
                subdestinations++;
            }
            if (cohesionBoids > 0)
            {
                cohesionDestination /= cohesionBoids;
                Debug.DrawLine(transform.position, cohesionDestination, Color.cyan);
                finalDestination += cohesionDestination / 8;
                subdestinations++;
            }
            Debug.DrawLine(transform.position, seperationDestination + alignmentDestination + cohesionDestination);
            if (nearbyAvoider)
            {
                Debug.DrawLine(transform.position, avoidDestination, Color.red);
                finalDestination += avoidDestination;
                subdestinations++;
            }
            if (nearbyAttractor)
            {
                Debug.DrawLine(transform.position, avoidDestination, Color.blue);
                finalDestination += attractDestination;
                subdestinations++;
            }
            //Debug.DrawLine(transform.position, finalDestination / subdestinations, Color.green);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(finalDestination / subdestinations - transform.position), steering);
        }
        body.velocity += transform.forward * acceleration;
        if (body.velocity.magnitude > speed) body.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Avoider")) Destroy(gameObject);
    }
}

