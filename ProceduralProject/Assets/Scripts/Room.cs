using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Transform[] wallPrefabs;
    public int roomSize = 5;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void InitRoom(RoomType type)
    {
        // spawn walls
        Transform prefabN = wallPrefabs[Random.Range(0, wallPrefabs.Length)];
        Transform prefabS = wallPrefabs[Random.Range(0, wallPrefabs.Length)];
        Transform prefabE = wallPrefabs[Random.Range(0, wallPrefabs.Length)];
        Transform prefabW = wallPrefabs[Random.Range(0, wallPrefabs.Length)];
        float dis = roomSize / 2 - 0.25f;
        // pick positions:
        Vector3 posN = new Vector3(0, 0, +dis) + transform.position;
        Vector3 posS = new Vector3(0, 0, -dis) + transform.position;
        Vector3 posE = new Vector3(+dis, 0, 0) + transform.position;
        Vector3 posW = new Vector3(-dis, 0, 0) + transform.position;

        Quaternion rotN = Quaternion.Euler(-90, (Random.Range(0, 100) < 50) ? 0 : 180, 0);
        Quaternion rotS = Quaternion.Euler(-90, (Random.Range(0, 100) < 50) ? 0 : 180, 0);
        Quaternion rotE = Quaternion.Euler(-90, (Random.Range(0, 100) < 50) ? 0 : 180, 0);
        Quaternion rotW = Quaternion.Euler(-90, (Random.Range(0, 100) < 50) ? 0 : 180, 0);

        // spawn walls
        Instantiate(prefabN, posN, rotN, transform);
    }
}
