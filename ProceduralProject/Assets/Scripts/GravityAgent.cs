using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAgent : MonoBehaviour
{
    static float G = 1;
    static float MAX_FORCE = 100;
    static List<GravityAgent> agents = new List<GravityAgent>();

    public bool sun;
    static void FindGravityForce(GravityAgent a, GravityAgent b)
    {
        if (a == b || a.isDone || b.isDone) return;

        Vector3 vectorToB = b.position - a.position;
        float gravity
            = G
            * (a.mass * b.mass)
            / vectorToB.sqrMagnitude;

        if (gravity > MAX_FORCE) gravity = MAX_FORCE;

        vectorToB.Normalize();

        a.AddForce(vectorToB * gravity);
        b.AddForce(-vectorToB * gravity);
    }

    Vector3 position;
    Vector3 force;
    Vector3 velocity;

    float mass;
    bool isDone = false;

    // Start is called before the first frame update
    void Start()
    {
        if (sun)
        {
            SetParameters(Vector3.zero, 1000, Color.yellow);
            for (int i = 0; i < 30; i++) Instantiate(gameObject).GetComponent<GravityAgent>().sun = false;
        }
        else SetParameters(new Vector3(
            Random.Range(-54, 54),
            Random.Range(-54, 54),
            Random.Range(-54, 54)),
            Random.Range(10f, 100f),
            new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)));
    }
    private void SetParameters(Vector3 startingPosition, float startingMass, Color color)
    {
        transform.position = startingPosition;
        position = transform.position;
        mass = startingMass;
        transform.localScale = Vector3.one * Mathf.Sqrt(mass);
        GetComponent<Renderer>().material.color = color;
        agents.Add(this);
    }
    private void OnDestroy() => agents.Remove(this);

    public void AddForce(Vector3 f) => force += f;

    // Update is called once per frame
    void Update()
    {
        // calc gravity to every other agent:
        foreach (GravityAgent a in agents)
            FindGravityForce(this, a);
        isDone = true;

        // euler integration:
        velocity += force / mass * Time.deltaTime;
        position += velocity * Time.deltaTime;

        transform.position = position;
    }
    private void LateUpdate()
    {
        isDone = false;
        force *= 0;
    }
}
