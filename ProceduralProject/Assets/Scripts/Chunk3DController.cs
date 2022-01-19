using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk3DController : MonoBehaviour
{
    public GameObject voxelPrefab;
    [Tooltip("How many voxels per dimension.")]
    public int dimensionSize = 10;
    [Tooltip("The size of a voxel, in meters.")]
    public float voxelSize = 1;
    public float threshold = 0.5f;
    public float zoom = 10;

    private void Start()
    {
        GenerateVoxels();
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        GenerateVoxels();
    }

    private void GenerateVoxels()
    {
        print("Generating voxels");
        if(!voxelPrefab) return;
        foreach (Transform child in transform) Destroy(child.gameObject);
        for (int x = 0; x < dimensionSize; x++) for (int y = 0; y < dimensionSize; y++) for (int z = 0; z < dimensionSize; z++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    float val = Noise.Perlin(pos / zoom);
                    if (val > threshold)
                    {
                        GameObject obj = Instantiate(voxelPrefab, pos, Quaternion.identity, transform);
                        obj.transform.localScale = Vector3.one * voxelSize;
                    }
                }
    }
}
