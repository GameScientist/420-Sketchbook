using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour
{
    private float speed;
    private float fullness;
    private Rigidbody body;
    private BoidManager manager;
    [SerializeField]
    private GameObject poopPrefab;
    private float poopTimer;
    private float poopBurstTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        speed = Random.Range(1f, 2f);
        fullness = Random.Range(10f, 100f);
        manager = BoidManager.singleton;
        poopTimer = Random.Range(1f, 6f);
    }

    // Update is called once per frame
    void Update()
    {
        fullness -= Time.deltaTime;
        if (fullness <= 0)
        {
            body.useGravity = true;
            if (fullness <= -5f)
            {
                manager.predators.Remove(this);
                Destroy(gameObject);
            }
            return;
        }
        Move();
        transform.rotation = Quaternion.LookRotation(body.velocity);
        if (poopTimer <= 0)
        {
            poopBurstTimer -= Time.deltaTime;
            if (poopBurstTimer <= 0)
            {
                manager.poop.Add(Instantiate(poopPrefab, transform.position - (transform.up/2), transform.rotation, null));
                if (Random.Range(0f, 1f) < .1f) poopTimer = Random.Range(1f, 6f);
                else poopBurstTimer = Random.Range(.1f, 1);
            }
        }
        else poopTimer -= Time.deltaTime;
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
        if (fullness <= 0) return;
        Prey prey = collision.gameObject.GetComponent<Prey>();
        if (prey != null) fullness += Random.Range(1f, 10f);
    }
}
