using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BranchingType
{
    Random,
    Opposite,
    Alternate,
    Alternate180,
    Alternate1375,
    WhorledTwo,
    WhorledThree
}

public class InstanceCollection
{
    public List<CombineInstance> branchInstances = new List<CombineInstance>();
    public List<CombineInstance> leafInstances = new List<CombineInstance>();

    public void AddBranch(Mesh mesh, Matrix4x4 xform)
    {
        branchInstances.Add(new CombineInstance() { mesh = mesh, transform = xform });
    }

    public void AddLeaf(Mesh mesh, Matrix4x4 xform)
    {
        leafInstances.Add(new CombineInstance() { mesh = mesh, transform = xform });
    }
    public Mesh MakeMultiMesh()
    {
        Mesh branchMesh = new Mesh();
        branchMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        branchMesh.CombineMeshes(branchInstances.ToArray());

        Mesh leafMesh = new Mesh();
        leafMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        leafMesh.CombineMeshes(branchInstances.ToArray());

        Mesh finalMesh = new Mesh();
        finalMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        finalMesh.CombineMeshes(new CombineInstance[]{
            new CombineInstance() { mesh = branchMesh, transform = Matrix4x4.identity },
            new CombineInstance() { mesh = leafMesh, transform = Matrix4x4.identity }
        }, false);

        return finalMesh;
    }
}
