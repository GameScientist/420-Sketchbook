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
        List<Vector3> destinations = new List<Vector3>();
        Boid[] boids = transform.parent.GetComponentsInChildren<Boid>();
        if (boids[0] != null)
        {
            destinations.Add(boids[0].transform.position);
            int i = destinations.Count - 1;
            foreach (Boid boid in boids) if (Vector3.Distance(transform.position, destinations[i]) > Vector3.Distance(transform.position, boid.transform.position)) destinations[i] = boid.transform.position;
        }
        GameObject[] nests = GameObject.FindGameObjectsWithTag("Avoiders");
        if (nests[0] != null)
        {
            destinations.Add(nests[0].transform.position);
            int i = destinations.Count - 1;
            foreach (GameObject nest in nests) if (Vector3.Distance(transform.position, destinations[i]) > Vector3.Distance(transform.position, nest.transform.position)) destinations[i] = nest.transform.position;
        }
        Vector3 destination;
        if (destinations.Count == 2) destination = (destinations[0] * 0.75f) + (destinations[1] / 4);
        else destination = destinations[0];
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(destination - transform.position), 1 - Mathf.Pow(.05f, Time.deltaTime));
        //body.velocity += transform.forward * Mathf.Lerp(1, acceleration, Mathf.Clamp01(Vector3.Distance(transform.position, destination) / 7));
        //if(body.velocity.magnitude > speed) body.velocity = transform.forward * 
    }
}
