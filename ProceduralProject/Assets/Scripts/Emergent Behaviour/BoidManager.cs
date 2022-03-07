using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public static BoidManager singleton { get; private set; }
    public List<Prey> preys;
    public List<Predator> predators;
    private void Awake()
    {
        if (singleton != null && singleton != this) Destroy(this);
        else singleton = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
