using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing2 : MonoBehaviour
{
    public Shader shader;
    private Material mat;

    public Texture noiseTexture;
    void Start()
    {
        mat = new Material(shader);

        mat.SetTexture("_Vignette", noiseTexture);
    }
    public void UpdateBloom(float bloom)
    {
        mat.SetFloat("_Bloom", bloom);
    }
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, mat);
    }
}
