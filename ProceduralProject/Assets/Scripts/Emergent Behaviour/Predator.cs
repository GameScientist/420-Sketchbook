using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
    private float speed;
    private float fullness;
    private float deathTime;
    private bool dead = false;
    private Rigidbody body;
    private BoidManager manager;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        speed = Random.Range(1f, 2f);
        fullness = Random.Range(10, 100);
        deathTime = Random.Range(3f, 5f);
        manager = BoidManager.singleton;
    }

    // Update is called once per frame
    void Update()
    {
        fullness -= Time.deltaTime;
        if (fullness <= 0) dead = true;
        if (dead)
        {
            deathTime -= Time.deltaTime;
            if (deathTime <= 0)
            {
                manager.predators.Remove(this);
                Destroy(gameObject);
            }
            return;
        }
        Move();
        transform.rotation = Quaternion.LookRotation(body.velocity);
    }

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

    private void OnCollisionEnter(Collision collision)
    {
        if (dead) return;
        Prey prey = collision.gameObject.GetComponent<Prey>();
        if (prey != null) fullness += Random.Range(1f, 10f);
    }
}
