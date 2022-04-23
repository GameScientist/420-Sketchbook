using UnityEngine;

/// <summary>
/// Lights up and changes colors to the beat of the music.
/// </summary>
public class Panel : MonoBehaviour
{
    /// <summary>
    /// When the user has their mouse over a panel.
    /// </summary>
    private bool mouseOver = false;
    /// <summary>
    /// The mesh of the panel.
    /// </summary>
    private MeshRenderer mesh;
    public bool wall, white;
    /// <summary>
    /// Controls the property of the shader.
    /// </summary>
    public float brightness = 0, hue = 0;
    /// <summary>
    /// The current color of this panel.
    /// </summary>
    public Color color = Color.black;
    /// <summary>
    /// How undesireable it is for a dancer to move across this panel.
    /// </summary>
    public int moveCost;

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
