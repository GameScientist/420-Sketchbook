using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing2 : MonoBehaviour
{
    /// <summary>
    /// The material created by this script.
    /// </summary>
    private Material mat;
    /// <summary>
    /// The shader used by the material.
    /// </summary>
    public Shader shader;
    /// <summary>
    /// The texture used for the material.
    /// </summary>
    public Texture vignette;

    void Start()
    {
        mat = new Material(shader);
        mat.SetTexture("_Vignette", vignette);
    }

    public void UpdateBloom(float bloom) => mat.SetFloat("_Bloom", bloom);

    void OnRenderImage(RenderTexture src, RenderTexture dst) => Graphics.Blit(src, dst, mat);
}
