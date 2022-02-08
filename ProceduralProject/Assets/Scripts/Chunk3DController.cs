using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk3DController : MonoBehaviour
{
    public GameObject lavaPrefab, portalPrefab, spawnPrefab, voxelPrefab;
    [Tooltip("How many voxels per dimension.")]
    public int dimensionSize = 10;
    [Tooltip("The size of a voxel, in meters.")]
    public float voxelSize = 1;
    public float threshold = 0.5f;
    private float zoom;

    private void Start()
    {
        zoom = Random.Range(5, 15);
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
        if (!voxelPrefab) return;
        foreach (Transform child in transform) Destroy(child.gameObject);
        for (int x = 0; x < dimensionSize; x++) for (int y = 0; y < dimensionSize; y++) for (int z = 0; z < dimensionSize; z++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    if (Noise.Perlin(pos / zoom) > threshold) SetupVoxel(Instantiate(voxelPrefab, pos, Quaternion.identity, transform));
                }
    }

    /// <summary>
    /// Sets the scale and color of the voxel.
    /// </summary>
    /// <param name="obj"></param>
    private void SetupVoxel(GameObject obj)
    {
        obj.transform.localScale = Vector3.one * voxelSize;
        obj.GetComponent<Renderer>().material.color = ChooseColoorLayer(obj.transform.position.y, dimensionSize / 2);
    }

    /// <summary>
    /// Fikgure out if the method for choosing the color of the voxel is built for upper voxels or lower voxels.
    /// </summary>
    /// <param name="voxelHeight">How high up this voxel is.</param>
    /// <param name="halfHeight">How high up voxels can be stacked before changing its coloring method.</param>
    /// <returns>The color of the voxel.</returns>
    private static Color ChooseColoorLayer(float voxelHeight, float halfHeight)
    {
        if (voxelHeight > halfHeight) return GenerateUpperVoxelColor(Random.Range(0, 40), (voxelHeight - halfHeight) / halfHeight + Random.Range(-0.1f, 0.1f));
        else return GenerateLowerVoxelColor(Random.Range(0, 121), voxelHeight / halfHeight + Random.Range(-0.1f, 0.1f));
    }

    /// <summary>
    /// Choose from a more limited selection of colors meant to represent ores, or color the block between brown or green depending on how high up the voxel is.
    /// </summary>
    /// <param name="rarity">Used to determine the type of ore choosen.</param>
    /// <param name="t">Used to lerp between the colors of normal voxels.</param>
    /// <returns></returns>
    private static Color GenerateUpperVoxelColor(int rarity, float t)
    {
        if (rarity == 0) return Color.green; // Emerald
        else if (rarity < 2) return new Color(0.75f, 0.75f, 0.75f); // Iron
        else if (rarity < 4) return new Color(0.6f, 0.4f, 0.2f); // Copper
        else if (rarity < 12) return Color.black; // Coal
        else return Color.Lerp(new Color(0.6f, 0.3f, 0), new Color(0, 0.6f, 0), t); // Dirt/Grass
    }

    /// <summary>
    /// Choose from a more expanded selection of colors meant to represent ores, or color the block between gray and brown depending on how high up the voxel is.
    /// </summary>
    /// <param name="rarity">Used to determine the type of ore choosen.</param>
    /// <param name="t">Used to lerp between the colors of normal voxels.</param>
    /// <returns></returns>
    private static Color GenerateLowerVoxelColor(int rarity, float t)
    {
        if (rarity == 0)
        {
            if (Random.value > 0.5f) return Color.cyan; // *DIAMOND*
            else return Color.yellow; // Gold
        }
        else if (rarity == 1) return Color.red; // Redstone
        else if (rarity == 2) return Color.green; // Emerald
        else if (rarity == 3) return Color.blue; // Lapis Lazuli
        else if (rarity < 8) return new Color(0.75f, 0.75f, 0.75f); // Iron
        else if (rarity < 13) return new Color(0.6f, 0.4f, 0.2f); // Copper
        else if (rarity < 40) return Color.black; // Coal
        else return Color.Lerp(Color.gray, new Color(0.6f, 0.3f, 0), t); // Stone/Dirt
    }
}
