using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class Orb : MonoBehaviour
{
    SimpleViz viz;
    Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        viz = SimpleViz.viz;
        GetComponent<MeshRenderer>().material.SetFloat("_TimeOffset", Random.Range(0, 255));
        body = GetComponent<Rigidbody>();
    }

    public void UpdateAudioData(float value)
    {
        transform.localScale = Vector3.one * (transform.localScale.x + value);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vToViz = viz.transform.position - transform.position;
        Vector3 dirToViz = vToViz.normalized;
        print(dirToViz);

        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, .01f);

        body.AddForce(dirToViz * 10000 * Time.deltaTime, ForceMode.Force);
    }
}
