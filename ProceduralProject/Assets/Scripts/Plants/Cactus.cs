using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
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

    private Transform player;
    private Quaternion playerDirection;
    private bool grown;

    private float Rand()
    {
        return (float)randGenerator.NextDouble();
    }
    private float Rand(float min, float max)
    {
        return Rand() * (max - min) + min;
    }
    private float RandBell(float min, float max)
    {
        // 6, 6
        // 2/12: (1/36)
        // 7: (1/6)
        // 2 to 12
        // 1 to 6
        min /= 2;
        return Rand(min, max) + Rand(min, max);
    }

    // Start is called before the first frame update
    void Start()
    {
        randGenerator = new System.Random(seed);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerDirection = Quaternion.LookRotation(player.transform.position) * Quaternion.Euler(90, 0, 0);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 10 && !grown)
        {
            grown = true;
            StartCoroutine(Grow(new InstanceCollection(), Vector3.zero, playerDirection, new Vector3(.25f, 2, .25f), iterations));
        }
    }

    IEnumerator Grow(InstanceCollection instances, Vector3 pos, Quaternion rot, Vector3 scale, int max, int num = 0, float nodeSpin = 0)
    {
        if (num < 0) num = 0;
        if (num >= max) yield break; // stop recursion

        yield return new WaitForSeconds(0.25f);

        // make a vube mess, add to list

        Matrix4x4 xform = Matrix4x4.TRS(pos, rot, scale);
        instances.AddBranch(MeshTools.MakeCube(), xform);

        // add to num calc percent
        float percentAtEnd = ++num / (float)max;
        Vector3 endPoint = xform.MultiplyPoint(new Vector3(0, 1, 0));

        if ((pos - endPoint).magnitude < .1f) yield break; // too small, stop recursion!

        bool hasNode = (num >= branchNodeTrunk && (num - branchNodeTrunk - 1) % branchNodeDistance == 0);

        if (hasNode)
        {
            if (branchingType == BranchingType.Alternate180) nodeSpin += 180f;
            if (branchingType == BranchingType.Alternate1375) nodeSpin += 137.5f;
        }

        {
            Quaternion randRod = rot * Quaternion.Euler(turnDegrees, twistDegrees, 0);
            Quaternion upRot = Quaternion.RotateTowards(rot, Quaternion.identity, 45);


            Quaternion newRot = Quaternion.Lerp(randRod, upRot, percentAtEnd);

            StartCoroutine(Grow(instances, endPoint, newRot, scale * .9f, max, num, nodeSpin));
        }

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

            float lean = Mathf.Lerp(90, 0, alignWithParent);
            for (int i = 0; i < howMany; i++)
            {
                float spin = nodeSpin + degreesBetweenNodes + 1;
                Quaternion newRot = rot * Quaternion.Euler(lean, spin, 0);

                float s = RandBell(.5f, .95f);

                StartCoroutine(Grow(instances, endPoint, newRot, scale * s, max, num, 90));
            }
        }
    }
}
