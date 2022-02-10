using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planter : MonoBehaviour
{
    private int berries = 0;
    [SerializeField]
    private GameObject cactusPrefab, berryPrefab, playerPrefab;
    private RandomSpawner[] spawners;
    private List<Cactus> cacti = new List<Cactus>();
    private List<Vector2> spawnPoints = new List<Vector2>();
    // Start is called before the first frame update
    void Start() => spawners = GetComponentsInChildren<RandomSpawner>();
    private void Update()
    {
        foreach (RandomSpawner spawner in spawners) if (!spawner.spawned) return;
        for (int x = -8; x < 16; x++) for (int y = -8; y < 16; y++) SpawnPlant(x, y);
        SpawnPlayer();
        Destroy(this);
    }

    private void SpawnPlant(int x, int y)
    {
        GameObject plant = PlantType();
        if (plant != null)
        {
            GameObject spawnedPlant = SpawnedPlant(x, y, plant);
            if (plant == cactusPrefab) cacti.Add(spawnedPlant.GetComponent<Cactus>());
        }
        else spawnPoints.Add(new Vector2(x, y));
    }

    private GameObject PlantType()
    {
        int plantRNG = Random.Range(0, 6);
        if (plantRNG == 5)
        {
            berries++;
            return berryPrefab;
        }
        else if (plantRNG >= 3) return cactusPrefab;
        else return null;
    }

    private static GameObject SpawnedPlant(int x, int y, GameObject plant)
    {
        Physics.SphereCast(new Vector3(Random.Range(x * 5, (x + 1) * 5), 80, Random.Range(y * 5, (y + 1) * 5)), 1, Vector3.down, out RaycastHit spawnPoint);
        return Instantiate(plant, spawnPoint.point, Quaternion.Euler(0, Random.Range(0, 359), 0));
    }

    private void SpawnPlayer()
    {
        Vector2 playerSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject player = SpawnedPlant((int)playerSpawnPoint.x, (int)playerSpawnPoint.y, playerPrefab);
        player.GetComponent<PlayerController>().berryGoal = berries;
        foreach (Cactus cactus in cacti) cactus.player = player.transform;
    }
}
