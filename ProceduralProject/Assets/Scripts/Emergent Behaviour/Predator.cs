using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
    private float acceleration;
    private float speed;
    private float stomachRoom;
    private float fullness;
    private float savoriness;
    private float deathTime;
    private bool dead = false;
    private Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        speed = Random.Range(0.1f, 4f);
        acceleration = speed * Random.Range(.1f, 1);
        stomachRoom = Random.Range(1f, 60f);
        fullness = stomachRoom / 2;
        savoriness = stomachRoom * Random.Range(.1f, 1f);
        deathTime = Random.Range(3f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (fullness <= 0)
        {
            dead = true;
            body.useGravity = true;
        }
        if (dead)
        {
            deathTime -= Time.deltaTime;
            if (deathTime <= 0) Destroy(gameObject);
            return;
        }
        bool foundDestination = false;
        Vector3 destination = new Vector3();
        Boid[] boids = transform.parent.GetComponentsInChildren<Boid>();
        GameObject[] nests = GameObject.FindGameObjectsWithTag("Avoider");
        if (boids.Length > 0 && fullness < stomachRoom)
        {
            destination = boids[0].transform.position;
            foreach (Boid boid in boids) if (Vector3.Distance(transform.position, destination) > Vector3.Distance(transform.position, boid.transform.position)) destination = boid.transform.position;
            if (!foundDestination) foundDestination = true;
        }
        else if (nests.Length > 0)
        {
            destination = nests[0].transform.position;
            foreach (GameObject nest in nests) if (Vector3.Distance(transform.position, destination) > Vector3.Distance(transform.position, nest.transform.position) && nest.GetComponent<Predator>() == null) destination = nest.transform.position;
            if (!foundDestination) foundDestination = true;
        }
        //Debug.DrawLine(transform.position, destination);
        if (foundDestination) transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(destination - transform.position), .9f);
        if (Vector3.Distance(transform.position, destination) > .2f) body.velocity += transform.forward * acceleration * Mathf.Abs(fullness - (stomachRoom / 2) / stomachRoom / 2);
        if (body.velocity.magnitude > speed)
        {
            body.velocity = transform.forward * speed;
            stomachRoom -= Time.deltaTime;
        }
        stomachRoom -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Boid>() != null) fullness += savoriness;
    }
}
