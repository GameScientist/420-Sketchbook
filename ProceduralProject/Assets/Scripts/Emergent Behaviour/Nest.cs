using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns fish. The type of fish spawned can be changed by the player.
/// </summary>
public class Nest : MonoBehaviour
{
    /// <summary>
    /// The time left before another fish is spawned.
    /// </summary>
    private float timer = 0;
    /// <summary>
    /// Has its nest lists adjusted.
    /// </summary>
    private BoidManager manager;
    /// <summary>
    /// The prefabs used to spawn the fish.
    /// </summary>
    [SerializeField]
    private GameObject predatorPrefab, preyPrefab;
    /// <summary>
    /// The material used on this nest.
    /// </summary>
    [SerializeField]
    private Material predatorMaterial, preyMaterial;
    /// <summary>
    /// If this nest spawns a predator or a prey.
    /// </summary>
    public bool predator;
    /// <summary>
    /// The key bind required to toggle the nest.
    /// </summary>
    public KeyCode keyBind;

    private void Start()
    {
        manager = BoidManager.singleton;
        Toggle();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)// When the timer runs out, a fish is spawned.
        {
            if (predator)// A predator is spawned.
            {
                Instantiate(predatorPrefab, transform.position + new Vector3(Random.Range(-.4f, .4f), Random.Range(-.4f, .4f), Random.Range(-.4f, .4f)), transform.rotation, null);
                timer = Random.Range(1f, 60f);
            }
            else// A prey is spawned.
            {
                Instantiate(preyPrefab, transform.position + new Vector3(Random.Range(-.4f, .4f), Random.Range(-.4f, .4f), Random.Range(-.4f, .4f)), transform.rotation, null);
                timer = Random.Range(1f, 6f);
            }
        }
        if (Input.GetKeyDown(keyBind))// Flip between predator and prey nest.
        {
            RemoveFromList();
            predator = !predator;
            Toggle();
        }
    }

    /// <summary>
    /// Switches between spawning predators and prey.
    /// </summary>
    private void Toggle()
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

    private void OnDestroy() => RemoveFromList();

    /// <summary>
    /// Removes the specified type of fish from the boid manager's list.
    /// </summary>
    private void RemoveFromList()
    {
        if (predator) manager.predatorNests.Remove(this);
        else manager.predatorNests.Remove(this);
    }
}
