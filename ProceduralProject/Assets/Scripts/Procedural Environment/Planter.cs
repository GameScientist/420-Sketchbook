using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planter : MonoBehaviour
{
    [SerializeField]
    private GameObject cactus, berry;
    private RandomSpawner[] spawners;
    // Start is called before the first frame update
    void Start()
    {
        spawners = GetComponentsInChildren<RandomSpawner>();
    }
    private void Update()
    {
        if (!ReadyToPlant()) return;
        for (int x = -4; x < 8; x++) for (int y = -4; y < 8; y++)
            {
                GameObject plant = PlantType();
                if (plant != null)
                {
                    Physics.Raycast(new Vector3(Random.Range(x * 10, (x + 1) * 10), 80, Random.Range(y * 10, (y + 1) * 10)), Vector3.down, out RaycastHit spawnPoint);
                    Instantiate(plant, spawnPoint.point, Quaternion.Euler(0, Random.Range(0, 359), 0));
                }
            }
        Destroy(this);
    }

    private bool ReadyToPlant()
    {
        foreach (RandomSpawner spawner in spawners) if (!spawner.spawned) return false;
        return true;
    }

    private GameObject PlantType()
    {
        int plantRNG = Random.Range(0, 6);
        if (plantRNG == 5) return berry;
        else if (plantRNG >= 3) return cactus;
        else return null;
    }
}
