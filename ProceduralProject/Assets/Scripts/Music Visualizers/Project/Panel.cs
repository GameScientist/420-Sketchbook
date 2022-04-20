using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public float brightness = 0;
    public float hue = 0;
    private MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hue >= 1) hue = 0;
        mesh.material.SetColor("_Color", Color.HSVToRGB(hue, 1, 1));
        if (brightness > 0)
        {
            mesh.material.SetFloat("_Emission", brightness);
            brightness -= Time.deltaTime;
            if (brightness < 0) brightness = 0;
        }
    }
}
