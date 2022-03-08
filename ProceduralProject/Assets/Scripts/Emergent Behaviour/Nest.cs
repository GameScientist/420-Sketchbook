using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    [SerializeField]
    private GameObject predatorPrefab;
    [SerializeField]
    private GameObject preyPrefab;
    [SerializeField]
    private Material predatorMaterial;
    [SerializeField]
    private Material preyMaterial;
    public bool predator;
    public Transform boidGroup;
    private float timer = 0;
    //private void OnValidate() => Toggle();

    private void Start() => Toggle();

    // Update is called once per frame
    void Update()
    {
        /*timer -= Time.deltaTime;
        if(timer <= 0)
        {
            if (predator)
            {
                Instantiate(predatorPrefab, transform.position + new Vector3(Random.Range(-.4f, .4f), Random.Range(-.4f, .4f), Random.Range(-.4f, .4f)), transform.rotation, boidGroup);
                timer = Random.Range(.1f, 60);
            }
            else
            {
                Instantiate(preyPrefab, transform.position + new Vector3(Random.Range(-.4f, .4f), Random.Range(-.4f, .4f), Random.Range(-.4f, .4f)), transform.rotation, boidGroup);
                timer = Random.Range(.1f, 8);
            }
        }*/
    }

    public void Toggle()
    {
        if (predator)
        {
            GetComponent<Renderer>().material = predatorMaterial;
            BoidManager.singleton.predatorNests.Add(this);
        }
        else
        {
            GetComponent<Renderer>().material = preyMaterial;
            BoidManager.singleton.preyNests.Add(this);
        }
    }
}
