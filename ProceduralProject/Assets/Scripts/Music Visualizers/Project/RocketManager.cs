using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketManager : MonoBehaviour
{
    public List<Rocket> rockets = new List<Rocket>();
    public static RocketManager singleton { get; private set; }
    private void OnValidate() => EstablishSingleton();
    private void Awake() => EstablishSingleton();
    private void EstablishSingleton()
    {
        if (singleton != null && singleton != this) Destroy(this);
        else singleton = this;
    }
}
