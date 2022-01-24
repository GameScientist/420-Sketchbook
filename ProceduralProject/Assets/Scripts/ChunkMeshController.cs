using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))] // tells Unity to 
public class ChunkMeshController : MonoBehaviour
{
    [Range(4, 40)]
    public int resolution = 10;

    [Range(5, 50)]
    public float zoom = 10;

    public float densityThreshold = 0.5f;

    private MeshFilter meshFilter;

    // Start is called before the first frame update
    void Start() => meshFilter = GetComponent<MeshFilter>();
    private void OnValidate() => BuildMesh(new bool[resolution, resolution, resolution]);
    void BuildMesh(bool[,,] voxels)
    {
        for (int x = 0; x < voxels.GetLength(0); x++) for (int y = 0; y < voxels.GetLength(1); y++) for (int z = 0; z < voxels.GetLength(2); z++) voxels[x, y, z] = Noise.Perlin(new Vector3(x, y, z) / zoom) > densityThreshold;
        //BuildMeshQuad();
        // Make storage for geometry.
        if (!meshFilter) meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = AddMesh(voxels, new List<Vector3>(), new List<int>(), new List<Vector3>(), new List<Vector2>());
    }

    private Mesh AddMesh(bool[,,] voxels, List<Vector3> verts, List<int> tris, List<Vector3> norms, List<Vector2> uvs)
    {
        // generating the geometry:
        for (int x = 0; x < voxels.GetLength(0); x++) for (int y = 0; y < voxels.GetLength(1); y++) for (int z = 0; z < voxels.GetLength(2); z++) if (voxels[x, y, z]) AddCubes(voxels, verts, tris, norms, uvs, x, y, z, 0);
        // Make mesh for geometry.
        return SetUpMesh(verts, tris, norms, uvs, new Mesh());
    }

    private Mesh SetUpMesh(List<Vector3> verts, List<int> tris, List<Vector3> norms, List<Vector2> uvs, Mesh mesh)
    {
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.normals = norms.ToArray();
        mesh.uv = uvs.ToArray();
        return mesh;
    }

    private void AddCubes(bool[,,] voxels, List<Vector3> verts, List<int> tris, List<Vector3> norms, List<Vector2> uvs, int x, int y, int z, byte sides)
    {
        if (!Lookup(voxels, x, y + 1, z)) sides |= 01;
        if (!Lookup(voxels, x, y - 1, z)) sides |= 02;
        if (!Lookup(voxels, x + 1, y, z)) sides |= 04;
        if (!Lookup(voxels, x - 1, y, z)) sides |= 08;
        if (!Lookup(voxels, x, y, z + 1)) sides |= 16;
        if (!Lookup(voxels, x, y, z - 1)) sides |= 32;

        AddCube(new Vector3(x, y, z), sides, verts, tris, norms, uvs);
    }

    bool Lookup(bool[,,] arr, int x, int y, int z)
    {
        if (x < 0) return false;
        if (y < 0) return false;
        if (z < 0) return false;
        if (x >= arr.GetLength(0)) return false;
        if (y >= arr.GetLength(1)) return false;
        if (z >= arr.GetLength(2)) return false;
        return arr[x, y, z];
    }

    void AddCube(Vector3 position, byte sides, List<Vector3> verts, List<int> tris, List<Vector3> norms, List<Vector2> uvs)
    {

        if ((sides & 01) > 0) AddQuad(position + new Vector3(0, +0.5f, 0), Quaternion.Euler(000, 0, 000), verts, tris, norms, uvs);
        if ((sides & 02) > 0) AddQuad(position + new Vector3(0, -0.5f, 0), Quaternion.Euler(000, 0, 180), verts, tris, norms, uvs);
        if ((sides & 04) > 0) AddQuad(position + new Vector3(+0.5f, 0, 0), Quaternion.Euler(000, 0, -90), verts, tris, norms, uvs);
        if ((sides & 08) > 0) AddQuad(position + new Vector3(-0.5f, 0, 0), Quaternion.Euler(000, 0, +90), verts, tris, norms, uvs);
        if ((sides & 16) > 0) AddQuad(position + new Vector3(0, 0, +0.5f), Quaternion.Euler(+90, 0, 000), verts, tris, norms, uvs);
        if ((sides & 32) > 0) AddQuad(position + new Vector3(0, 0, -0.5f), Quaternion.Euler(-90, 0, 000), verts, tris, norms, uvs);
    }

    void AddQuad(Vector3 position, Quaternion rotation, List<Vector3> verts, List<int> tris, List<Vector3> norms, List<Vector2> uvs)
    {
        float[] offsets = new[] { 0.5f, -0.5f };
        foreach (float x in offsets) foreach (float z in offsets) verts.Add(position + rotation * new Vector3(x, 0, z));
        foreach (int vertAdder in new[] { 0, 1, 3, 1, 2, 3 }) tris.Add(verts.Count + vertAdder);
        for(int i = 0; i<4; i++)norms.Add(rotation * new Vector3(0, 1, 0));
        foreach (int x in new[] { 1, 0 }) foreach (int y in new[] { 0, 1 }) uvs.Add(new Vector2(x, y));
    }

    void BuildMeshQuad()
    {
        Mesh mesh = new Mesh();

        // Geometry
        // set mesh's vertices
        Vector3[] verts = new Vector3[]
        {
            new Vector3(+0.5f, 0, +0.5f),
            new Vector3(+0.5f, 0, -0.5f),
            new Vector3(-0.5f, 0, +0.5f),
            new Vector3(-0.5f, 0, -0.5f)
        };

        // set mesh's triangles
        int[] tris = new int[]
        {
            0,1,3,
            1,2,3
        };

        // Shading
        // set mesh's normals
        Vector3[] norms = new Vector3[] {
            new Vector3(0,1,0),
            new Vector3(0,1,0),
            new Vector3(0,1,0),
            new Vector3(0,1,0)
        };

        // set mesh's UVs
        Vector2[] uvs = new Vector2[]
        {
            new Vector2(1,0),
            new Vector2(1,1),
            new Vector2(0,1),
            new Vector2(0,0)
        };

        // set mesh's color
        // Use color to distinguish biomes?

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.normals = norms;
        mesh.uv = uvs;

        meshFilter.mesh = mesh;
    }
}
