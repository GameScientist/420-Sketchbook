using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps a list of every nest, fish, and turd in the scene.
/// </summary>
public class BoidManager : MonoBehaviour
{
    public List<GameObject> poop = new List<GameObject>();
    public List<Nest> predatorNests = new List<Nest>();
    public List<Nest> preyNests = new List<Nest>();
    public List<Predator> predators = new List<Predator>();
    public List<Prey> preys = new List<Prey>();
    public static BoidManager singleton { get; private set; }
    private void OnValidate() => EstablishSingleton();
    private void Awake() => EstablishSingleton();
    private void EstablishSingleton()
    {
        if (singleton != null && singleton != this) Destroy(this);
        else singleton = this;
    }
}
