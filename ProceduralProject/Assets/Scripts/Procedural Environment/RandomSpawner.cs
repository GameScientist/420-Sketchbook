using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;

    // Start is called before the first frame update
    void Start() => Instantiate(objects[Random.Range(0, objects.Length)], transform.position, transform.rotation);
}
