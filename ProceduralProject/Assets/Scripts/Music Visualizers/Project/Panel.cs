using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public float brightness = 0;
    public float hue = 0;
    public Color color = Color.black;
    private MeshRenderer mesh;
    public bool white = false;
    private bool mouseOver = false;
    public int moveCost;
    public bool wall;
    // Start is called before the first frame update
    void Start() => mesh = GetComponent<MeshRenderer>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mouseOver)
            {
                white = true;
                foreach (Dancer dancer in LightShow.singleton.dancers) dancer.destination = transform.position;
            }
            else white = false;
        }
        if(white) mesh.material.SetColor("_Color", Color.white);
        else mesh.material.SetColor("_Color", color);
        if(white) mesh.material.SetFloat("_Emission", 1);
        else if (brightness > 0)
        {
            mesh.material.SetFloat("_Emission", brightness);
            brightness -= Time.deltaTime;
            if (brightness < 0) brightness = 0;
        }
    }

    private void OnMouseEnter() => mouseOver = true;

    private void OnMouseExit() => mouseOver = false;
}
