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
    private float timer = 0;
    private BoidManager manager;
    public KeyCode keyBind;
    //private void OnValidate() => Toggle();

    private void Start()
    {
        manager = BoidManager.singleton;
        Toggle();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            if (predator)
            {
                Instantiate(predatorPrefab, transform.position + new Vector3(Random.Range(-.4f, .4f), Random.Range(-.4f, .4f), Random.Range(-.4f, .4f)), transform.rotation, null);
                timer = Random.Range(.1f, 60);
            }
            else
            {
                Instantiate(preyPrefab, transform.position + new Vector3(Random.Range(-.4f, .4f), Random.Range(-.4f, .4f), Random.Range(-.4f, .4f)), transform.rotation, null);
                timer = Random.Range(.1f, 8);
            }
        }
        if (Input.GetKeyDown(keyBind))
        {
            RemoveFromList();
            predator = !predator;
            Toggle();
        }
    }

    public void Toggle()
    {
        if (predator)
        {
            GetComponent<Renderer>().material = predatorMaterial;
            manager.predatorNests.Add(this);
        }
        else
        {
            GetComponent<Renderer>().material = preyMaterial;
            manager.preyNests.Add(this);
        }
    }

    private void OnDestroy()
    {
        RemoveFromList();
    }

    private void RemoveFromList()
    {
        if (predator) manager.predatorNests.Remove(this);
        else manager.predatorNests.Remove(this);
    }
}
