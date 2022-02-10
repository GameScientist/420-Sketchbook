using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Cactus : MonoBehaviour
{
    [Range(0, 100000000)]
    private int seed = 0;

    [Range(5, 30)]
    public int iterations = 5, turnDegrees = 10;

    [Range(-45, 45)]
    public int twistDegrees = 10;

    [Range(0, 1)]
    public float alignWithParent;

    [Range(1, 10)]
    public int branchNodeDistance = 2, branchNodeTrunk = 1;

    public BranchingType branchingType;

    private System.Random randGenerator;

    private bool grown;
    public Transform player;

    private void Update()
    {
        if (player == null) return;
        if (Vector3.Distance(transform.position, player.transform.position) < 7.5f && !grown)
        {
            grown = true;
            StartCoroutine(Grow(new InstanceCollection(), Vector3.zero, PlayerDirection(), new Vector3(.5f, 1, .5f), iterations));
        }
    }

    IEnumerator Grow(InstanceCollection instances, Vector3 pos, Quaternion rot, Vector3 scale, int max, int num = 0, float nodeSpin = 0)
    {
        if (num < 0) num = 0;
        if (num >= max) yield break; // stop recursion

        // add to num calc percent
        float percentAtEnd = ++num / (float)max;

        yield return new WaitForSeconds(Mathf.Lerp(0.017f, 0.25f, percentAtEnd) + Random.Range(-0.15f, 0.15f));

        // make a vube mess, add to list

        Matrix4x4 xform = Matrix4x4.TRS(pos, rot, scale);
        instances.AddBranch(MeshTools.MakeCube(), xform);

        Vector3 endPoint = xform.MultiplyPoint(new Vector3(0, 1, 0));

        if ((pos - endPoint).magnitude < .1f) yield break; // too small, stop recursion!

        bool hasNode = num >= branchNodeTrunk && (num - branchNodeTrunk - 1) % branchNodeDistance == 0;

        if (hasNode)
        {
            if (branchingType == BranchingType.Alternate180) nodeSpin += 180f;
            if (branchingType == BranchingType.Alternate1375) nodeSpin += 137.5f;
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.CombineMeshes(instances.branchInstances.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter) meshFilter.mesh = instances.MakeMultiMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;

        StartCoroutine(Grow(
            instances,
            endPoint,
            Quaternion.Lerp(rot * Quaternion.Euler(turnDegrees, twistDegrees, 0), Quaternion.RotateTowards(rot, PlayerDirection(), 45), percentAtEnd),
            scale * .9f,
            max,
            num,
            nodeSpin));

        if (hasNode)
        {
            int howMany = 0;
            float degreesBetweenNodes = 0;
            switch (branchingType)
            {
                case BranchingType.Random:
                    howMany = 1;
                    break;
                case BranchingType.Opposite:
                    howMany = 2;
                    degreesBetweenNodes = 180;
                    break;
                case BranchingType.Alternate180:
                    howMany = 1;

                    break;
                case BranchingType.Alternate1375:
                    howMany = 1;

                    break;
                case BranchingType.WhorledTwo:
                    howMany = 2;
                    degreesBetweenNodes = 180;
                    break;
                case BranchingType.WhorledThree:
                    howMany = 3;
                    degreesBetweenNodes = 120;
                    break;
            }

            for (int i = 0; i < howMany; i++) StartCoroutine(Grow(
                instances,
                endPoint,
                rot * Quaternion.Euler(Mathf.Lerp(90, 0, alignWithParent), nodeSpin + degreesBetweenNodes + 1, 0),
                scale * 0.9f,
                max,
                num,
                90));
        }
    }

    Quaternion PlayerDirection() => Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up) * Quaternion.Euler(90, 0, 0);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) other.GetComponent<PlayerController>().GameOver(false);
    }
}
