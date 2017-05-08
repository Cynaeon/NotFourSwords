using UnityEngine;
using System.Collections;

public class FogDensity : MonoBehaviour
{
    public bool fog;
    public Color fogColor;
    public float fogDensity;
    public Color ambientLight;
    public float haloStrength;
    public float flareStrength;

    bool previousFog;
    Color previousFogColor;
    float previousFogDensity;
    Color previousAmbientLight;
    float previousHaloStrength;
    float previousFlareStrength;
    private Camera playerCamera;
    public float clipPlane;
    private bool fade;

    
    void Start()
    {
        /*
        playerCamera = GetComponent<Camera>();
        clipPlane = 2000;
        fogDensity = 0.05f;
        RenderSettings.fogColor = Color.magenta;
        */
    }

    void Update()
    {
        
    }

    public void fadeState(bool state)
    {
        fade = state;
    }

    void OnPreRender()
    {
        previousFogDensity = RenderSettings.fogDensity;
        previousFogColor = RenderSettings.fogColor;
        if (fade)
        {
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogDensity = 0.05f;
        }
    }

    private void OnPostRender()
    {
        RenderSettings.fogDensity = previousFogDensity;
        RenderSettings.fogColor = previousFogColor;
    }
}


