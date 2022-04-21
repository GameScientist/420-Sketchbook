using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceFloor : MonoBehaviour
{
    Panel[] panels = new Panel[128];
    [SerializeField]
    private Material unlit;
    [SerializeField]
    private Material lit;
    private AudioSource player;
    private int numBands = 512;
    private Light light;
    private float hue = 0;
    private LineRenderer line;
    [SerializeField]
    private MeshRenderer[] dancers;
    private float flipTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<AudioSource>();
        panels = GetComponentsInChildren<Panel>();
        light = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        flipTimer -= Time.deltaTime;
        float[] bands = new float[numBands];
        player.GetSpectrumData(bands, 0, FFTWindow.BlackmanHarris);

        float avgAmp = 0;
        for (int i = 0; i < panels.Length; i++)
        {
            avgAmp += bands[i];
        }
        avgAmp /= numBands;
        avgAmp *= 100;
        hue += avgAmp * Time.deltaTime;
        if (hue >= 1) hue -= 1;

        int panelSamples = 128;
        int dancerSamples = 8;
        float[] panelData = new float[panelSamples];
        float[] dancerData = new float[dancerSamples];
        player.GetOutputData(panelData, 0);
        for (int i = 0; i < panels.Length; i++)
        {
            if (panelData[i] > 0.5f) panels[i].brightness = 1;
            panels[i].color = Color.HSVToRGB(hue, 1, 1);
        }
        player.GetOutputData(dancerData, 0);
        for(int i = 0; i<dancers.Length; i++)
        {
            if (panelData[i] > 0.3f && flipTimer <= 0)
            {
                flipTimer = 0.25f;
                if (dancers[i].material.GetFloat("_Flip") == 0) dancers[i].material.SetFloat("_Flip", 1);
                else dancers[i].material.SetFloat("_Flip", 0);
            }
        }
        light.spotAngle = avgAmp * 100;
        float lightHue = hue + 0.5f;
        if (lightHue >= 1) lightHue -= 1;
        light.color = Color.HSVToRGB(lightHue, 1, 1);
    }
}
