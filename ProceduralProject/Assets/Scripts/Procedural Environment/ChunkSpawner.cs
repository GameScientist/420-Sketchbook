using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject chunkPrefab;

    public void SpawnChunk(float densityThreshold, Vector3 offset, float zoom, float flattenAmount, float flattenOffset)
    {
        ChunkMeshController chunk = Instantiate(chunkPrefab, transform.position, transform.rotation).GetComponent<ChunkMeshController>();
        chunk.densityThreshold = densityThreshold;
        chunk.noiseFields = new ChunkMeshController.NoiseField[2];
        chunk.noiseFields[0] = NewNoiseField(ChunkMeshController.BlendMode.Add, offset, zoom, flattenAmount, flattenOffset);
        int blendIndex = Random.Range(0, 3);
        chunk.noiseFields[1] = NewNoiseField(RandomBlend(blendIndex), new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), Random.Range(5f, 50f), Random.Range(0f, 1f), Random.Range(-50f, 50f));
        chunk.BuildMesh(new bool[40, 40, 40]);
    }

    private static ChunkMeshController.NoiseField NewNoiseField(ChunkMeshController.BlendMode blend, Vector3 offset, float zoom, float flattenAmount, float flattenOffset)
    {
        ChunkMeshController.NoiseField noiseField = new();
        noiseField.blend = blend;
        noiseField.offset = offset;
        noiseField.zoom = zoom;
        noiseField.flattenAmount = flattenAmount;
        noiseField.flattenOffset = flattenOffset;
        return noiseField;
    }

    private static ChunkMeshController.BlendMode RandomBlend(int blendIndex)
    {
        ChunkMeshController.BlendMode blend = ChunkMeshController.BlendMode.Add;
        switch (blendIndex)
        {
            case 0:
                blend = ChunkMeshController.BlendMode.Average;
                break;
            case 1:
                blend = ChunkMeshController.BlendMode.Mult;
                break;
            case 2:
                blend = ChunkMeshController.BlendMode.Sub;
                break;
        }

        return blend;
    }
}
