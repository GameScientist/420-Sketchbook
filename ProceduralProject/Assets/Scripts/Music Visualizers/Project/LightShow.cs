using UnityEngine;

/// <summary>
/// Changes the effects of the scene to the beat of the music.
/// </summary>
public class LightShow : MonoBehaviour
{
    /// <summary>
    /// Controls the color of the many effects of the scene.
    /// </summary>
    private float hue = 0;
    /// <summary>
    /// How much time each dancer has left before their shaders are flipped again.
    /// </summary>
    private float[] flipTimers;
    private int numBands = 512;
    /// <summary>
    /// The source of the music being played.
    /// </summary>
    private AudioSource player;
    /// <summary>
    /// Have their shaders flipped by this script.
    /// </summary>
    public Dancer[] dancers;
    /// <summary>
    /// The parent game objects of the lights and panels of the scene.
    /// </summary>
    [SerializeField]
    private GameObject lightGroup, panelGroup;
    /// <summary>
    /// The lights that have their hue altered by this script.
    /// </summary>
    private Light[] lights;
    public static LightShow singleton { get; private set; }
    /// <summary>
    /// All of the panels that have their hues altered.
    /// </summary>
    private Panel[] panels = new Panel[128];
    /// <summary>
    /// Gives off a white flash effect for the camera.
    /// </summary>
    private PostProcessing2 processer;

    private void Awake()
    {
        if (singleton != null) // we already have a singleton...
        {
            Destroy(gameObject);
            return;
        }

        singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        flipTimers = new float[dancers.Length];
        lights = lightGroup.GetComponentsInChildren<Light>();
        panels = panelGroup.GetComponentsInChildren<Panel>();
        player = GetComponent<AudioSource>();
        processer = GetComponent<PostProcessing2>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpectrumData();
        UpdatePanelShader();
        MakeDancersDance();
    }

    /// <summary>
    /// Updates the hue and post processing effects of the scene based on the spectrum data.
    /// </summary>
    private void UpdateSpectrumData()
    {
        float[] bands = new float[numBands];
        float avgAmp = 0;
        player.GetSpectrumData(bands, 0, FFTWindow.BlackmanHarris);
        for (int i = 0; i < panels.Length; i++) avgAmp += bands[i];
        avgAmp /= numBands;

        processer.UpdateBloom(avgAmp * 200);

        hue += avgAmp * Time.deltaTime * 200;
        if (hue >= 1) hue -= 1;
        foreach (Light light in lights)
        {
            light.spotAngle = avgAmp / 30000;
            light.color = Color.HSVToRGB(hue, 1, 1);
        }
    }

    /// <summary>
    /// Updates the hue and brightness of each panel in the scene.
    /// </summary>
    private void UpdatePanelShader()
    {
        float[] panelData = new float[128];
        player.GetOutputData(panelData, 0);
        for (int i = 0; i < panels.Length; i++)
        {
            if (panelData[i] > 0.5f) panels[i].brightness = 1;
            panels[i].color = Color.HSVToRGB(hue, 1, 1);
        }
    }

    /// <summary>
    /// Makes the dancers flip their shaders.
    /// </summary>
    private void MakeDancersDance()
    {
        float[] dancerData = new float[8];
        player.GetOutputData(dancerData, 0);
        for (int i = 0; i < dancers.Length; i++)
        {
            flipTimers[i] -= Time.deltaTime;
            if (dancerData[i] > 0.3f && flipTimers[i] <= 0)
            {
                flipTimers[i] = 0.25f;
                if (dancers[i].GetComponent<MeshRenderer>().material.GetFloat("_Flip") == 0) dancers[i].GetComponent<MeshRenderer>().material.SetFloat("_Flip", 1);
                else dancers[i].GetComponent<MeshRenderer>().material.SetFloat("_Flip", 0);
            }
        }
    }
}
