using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public float brightness = 0;
    public float hue = 0;
    public Color color = Color.black;
    private MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mesh.material.SetColor("_Color", color);
        if (brightness > 0)
        {
            mesh.material.SetFloat("_Emission", brightness);
            brightness -= Time.deltaTime;
            if (brightness < 0) brightness = 0;
        }
    }
}
