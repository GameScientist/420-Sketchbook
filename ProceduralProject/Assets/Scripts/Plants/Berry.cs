using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Berry : MonoBehaviour
{
    [Range(2, 30)]
    public int iterations = 5;

    [Range(5, 30)]
    public int spreadDegress = 10;

    // Start is called before the first frame update
    void Start()
    {
        Build();
    }

    private void OnValidate()
    {
        Build();
    }

    void Build()
    {
        // 1. making storage for instances
        List<CombineInstance> instances = new List<CombineInstance>();

        // 2. spawn the instances

        Grow(instances, Vector3.zero, Quaternion.identity, new Vector3(.25f, .25f, .25f), iterations);

        // 3. combining the instances together
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(instances.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter) meshFilter.mesh = mesh;

    }

    void Grow(List<CombineInstance> instances, Vector3 pos, Quaternion rot, Vector3 scale, int max, int num = 0)
    {
        if (num < 0) num = 0;
        if (num >= max) return; // stop recursion

        // make a vube mess, add to list

        CombineInstance inst = new CombineInstance();
        inst.mesh = MeshTools.MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, scale);
        instances.Add(inst);

        // add to num calc percent
        float percent = ++num / (float)max;

        Vector3 endPoint = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));

        if ((pos - endPoint).magnitude < .1f) return; // too small, stop recursion!

        {
            Quaternion randRod = rot * Quaternion.Euler(spreadDegress, Random.Range(-90f, 90f), 0);
            Quaternion upRot = Quaternion.RotateTowards(rot, Quaternion.identity, 45);
            Quaternion newRot = Quaternion.Lerp(randRod, upRot, percent);
            Grow(instances, endPoint, newRot, scale * .9f, max, num + 1);
        }

        if (num > 1) if (num % 2 == 1)
            {
                float degrees = Random.Range(-1, 2) * 90;
                Quaternion newRot = Quaternion.LookRotation(endPoint - transform.position) * Quaternion.Euler(0, degrees, 0);
                Grow(instances, endPoint, newRot, scale * .9f, max, num + 1);
            }
    }
}
