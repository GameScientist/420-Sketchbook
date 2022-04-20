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
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<AudioSource>();
        panels = GetComponentsInChildren<Panel>();
    }

    // Update is called once per frame
    void Update()
    {
        float[] bands = new float[numBands];
        player.GetSpectrumData(bands, 0, FFTWindow.BlackmanHarris);

        float avgAmp = 0;
        for(int i = 0; i<panels.Length; i++)
        {
            avgAmp += bands[i];
        }
        avgAmp /= numBands;
        avgAmp *= 100;

        int samples = 128;
        float[] data = new float[samples];
        player.GetOutputData(data, 0);
        for (int i=0; i<panels.Length; i++)
        {
            if (data[i]>0.5f) panels[i].brightness = 1;
            panels[i].hue += avgAmp * Time.deltaTime;
        }

    }
}
