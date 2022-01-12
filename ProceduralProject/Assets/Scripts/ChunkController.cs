using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField]
    private GameObject voxelPrefab;
    public int chunkSize;

    public Vector2 offset;
    public float zoom = 20;
    public float amp = 10;
    // Start is called before the first frame update
    void Start()
    {
        BuildChunk();
    }

    private void BuildChunk()
    {
        if (!voxelPrefab) return;

        for(int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {

                Instantiate(voxelPrefab, new Vector3(x, Mathf.PerlinNoise(x/zoom, z/zoom)*amp, z), Quaternion.identity, transform);
            }
        }
    }
}
