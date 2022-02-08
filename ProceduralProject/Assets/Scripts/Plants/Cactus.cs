using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Cactus : MonoBehaviour
{
    [Range(2, 20)]
    public int iterations = 5;

    [Range(5, 45)]
    public int spreadDegress = 10;

    public Transform player;

    static private Quaternion playerDirection;

    private bool grown;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerDirection = Quaternion.LookRotation(player.transform.position) * Quaternion.Euler(90, 0, 0);
        Build();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 10 && !grown) StartCoroutine(Grow(new List<CombineInstance>(), Vector3.zero, playerDirection, new Vector3(.25f, 1, .25f), iterations));
    }

    void Build()
    {
        StartCoroutine(Grow(new List<CombineInstance>(), Vector3.zero, playerDirection, new Vector3(.25f, 1, .25f), iterations));
    }

    IEnumerator Grow(List<CombineInstance> instances, Vector3 pos, Quaternion rot, Vector3 scale, int max, int num = 0)
    {
        if (num < 0) num = 0;
        if (num >= max) yield break; // stop recursion

        yield return new WaitForSeconds(0.02f);

        // make a vube mess, add to list

        CombineInstance inst = new CombineInstance();
        inst.mesh = MeshTools.MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, scale);
        instances.Add(inst);

        Vector3 endPoint = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(instances.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter) meshFilter.mesh = mesh;

        if ((pos - endPoint).magnitude < .1f) yield break; // too small, stop recursion!

        StartCoroutine(Grow(instances, endPoint, Quaternion.Lerp(Quaternion.RotateTowards(rot, playerDirection, 45), rot * Quaternion.Euler(spreadDegress, Random.Range(-90f, 90f), 0), ++num / (float)max), scale * .9f, max, num + 1));

        if (num > 1) if (num % 2 == 1) StartCoroutine(Grow(instances, endPoint, playerDirection * Quaternion.Euler(0, Random.Range(-1, 2) * 90, 0), scale * .9f, max, num + 1));
    }
}
