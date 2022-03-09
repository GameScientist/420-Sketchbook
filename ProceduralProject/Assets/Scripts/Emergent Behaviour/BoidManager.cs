using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public static BoidManager singleton { get; private set; }
    public List<Prey> preys = new List<Prey>();
    public List<Nest> preyNests = new List<Nest>();
    public List<Predator> predators = new List<Predator>();
    public List<Nest> predatorNests = new List<Nest>();
    public List<GameObject> poop = new List<GameObject>();
    private void OnValidate() => EstablishSingleton();
    private void Awake() => EstablishSingleton();

    private void EstablishSingleton()
    {
        if (singleton != null && singleton != this) Destroy(this);
        else singleton = this;
    }
}
