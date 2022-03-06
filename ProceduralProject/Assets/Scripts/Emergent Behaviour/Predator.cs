using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
    private float acceleration;
    private float speed;
    private Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        speed = Random.Range(0.1f, 6f);
        acceleration = speed * Random.Range(.1f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        bool foundDestination = false;
        Vector3 destination = new Vector3();
        Boid[] boids = transform.parent.GetComponentsInChildren<Boid>();
        GameObject[] nests = GameObject.FindGameObjectsWithTag("Avoider");
        if (boids[0] != null)
        {
            destination = boids[0].transform.position;
            foreach (Boid boid in boids) if (Vector3.Distance(transform.position, destination) > Vector3.Distance(transform.position, boid.transform.position)) destination = boid.transform.position;
            foundDestination = true;
        }
        else if (nests[0] != null)
        {
            destination = nests[0].transform.position;
            foreach (GameObject nest in nests) if (Vector3.Distance(transform.position, destination) > Vector3.Distance(transform.position, nest.transform.position)) destination = nest.transform.position;
            foundDestination = true;
        }
        if (foundDestination) transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(destination - transform.position), .9f);
        RaycastHit hit;
        Physics.Raycast(transform.position, destination - transform.position, out hit);
        Debug.DrawLine(transform.position, destination);
        if (Vector3.Distance(transform.position, hit.point) > .2f) body.velocity += transform.forward * acceleration;
        if (body.velocity.magnitude > speed) body.velocity = transform.forward * speed;
    }
}
