using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planter : MonoBehaviour
{
    private ChunkSpawner[] spawners;
    [SerializeField]
    private GameObject cactusPrefab, berryPrefab, lavaPrefab, portalPrefab, playerPrefab;
    private List<Cactus> cacti = new List<Cactus>();
    private List<Vector2> spawnPoints = new List<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        spawners = GetComponentsInChildren<ChunkSpawner>();
        Instantiate(
            lavaPrefab,
            new Vector3(Random.Range(-40f, 80f),
            80f, Random.Range(-40f, 80f)),
            transform.rotation);
        ActivateChunks();
        for (int x = -4; x < 8; x++) for (int y = -4; y < 8; y++) SpawnPlant(x, y);
        SpawnPlayer();
        SpawnPortal(Random.Range(0, spawnPoints.Count));
    }

    private void ActivateChunks()
    {
        Vector3 thresholdOffset = CubedNoiseOffset();
        Vector3 zoomOffset = CubedNoiseOffset();
        Vector3 flattenAmountOffset = CubedNoiseOffset();
        Vector3 flattenOffsetOffset = CubedNoiseOffset();
        foreach (ChunkSpawner spawner in spawners)
        {
            Vector3 noisePos = spawner.transform.position - new Vector3(40, 40, 40) / 120;
            spawner.SpawnChunk(
                Mathf.Lerp(0f, 1f, Noise.Perlin(noisePos + thresholdOffset)),
                CubedNoiseOffset(),
                Mathf.Lerp(5f, 50f, Noise.Perlin(noisePos + zoomOffset)),
                Mathf.Lerp(0f, 1f, Noise.Perlin(noisePos + flattenAmountOffset)),
                Mathf.Lerp(-50f, 50f, Noise.Perlin(noisePos + flattenOffsetOffset)));
        }
    }

    private static Vector3 CubedNoiseOffset() => new Vector3(Random01(), Random01(), Random01());

    private static float Random01() => Random.Range(0f, 1f);

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
        if (plantRNG == 5) return berryPrefab;
        else if (plantRNG >= 3) return cactusPrefab;
        else return null;
    }

    private static GameObject SpawnedPlant(int x, int y, GameObject plant)
    {
        Physics.SphereCast(
            new Vector3(Random.Range(x * 10, (x + 1) * 10),
            80,
            Random.Range(y * 10, (y + 1) * 10)),
            1,
            Vector3.down,
            out RaycastHit spawnPoint);
        return Instantiate(plant, spawnPoint.point, Quaternion.Euler(0, Random.Range(0, 359), 0));
    }

    private void SpawnPlayer()
    {
        Transform player = PlayerController.singleton.transform;
        int spawnPointIndex = Random.Range(0, spawnPoints.Count);
        Vector2 spawnPoint = spawnPoints[spawnPointIndex];
        int x = (int)spawnPoint.x;
        int y = (int)spawnPoint.y;
        Physics.SphereCast(
            new Vector3(Random.Range(x * 5, (x + 1) * 5),
            80,
            Random.Range(y * 5, (y + 1) * 5)),
            1,
            Vector3.down,
            out RaycastHit spawnPos);
        player.transform.position = spawnPos.point + transform.up * 2;
        spawnPoints.RemoveAt(spawnPointIndex);
        foreach (Cactus cactus in cacti) cactus.player = player.transform;
        player.GetComponent<PlayerController>().panel.SetActive(false);
    }

    GameObject SpawnPortal(int spawnPointIndex)
    {
        Vector2 spawnPoint = spawnPoints[spawnPointIndex];
        return SpawnedPlant((int)spawnPoint.x, (int)spawnPoint.y, portalPrefab);
    }
}
