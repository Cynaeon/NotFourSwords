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
        playerCamera = GetComponent<Camera>();
        clipPlane = 2000;
        fogDensity = 0;
    }

    void Update()
    {
        if (fade)
        {
            FadeFogIn();
        }
        else
        {
            FadeFogOut();
        }
    }

    public void fadeState(bool state)
    {
        fade = state;
    }

    private void FadeFogIn()
    {
        if (fogDensity <= 0.05)
        { 
            fogDensity = fogDensity + 0.005f;
        }
        if(clipPlane > 80)
        {
            clipPlane = clipPlane - 92;
        }
        playerCamera.farClipPlane = clipPlane;
        playerCamera.clearFlags = CameraClearFlags.SolidColor;
    }

    private void FadeFogOut()
    {
        if (fogDensity > 0)
        {
            fogDensity = fogDensity - 0.005f;
        }

        if (clipPlane < 1000)
        {
            clipPlane = clipPlane + 92;
        }
        playerCamera.farClipPlane = clipPlane;
        playerCamera.clearFlags = CameraClearFlags.SolidColor;
    }


    void OnPreRender()
    {
        previousFog = RenderSettings.fog;
        previousFogColor = RenderSettings.fogColor;
        previousFogDensity = RenderSettings.fogDensity;
        previousAmbientLight = RenderSettings.ambientLight;
        previousHaloStrength = RenderSettings.haloStrength;
        previousFlareStrength = RenderSettings.flareStrength;
        if (fog)
        {
            RenderSettings.fog = fog;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.ambientLight = ambientLight;
            RenderSettings.haloStrength = haloStrength;
            RenderSettings.flareStrength = flareStrength;
        }
    }

    void OnPostRender()
    {
        RenderSettings.fog = previousFog;
        RenderSettings.fogColor = previousFogColor;
        RenderSettings.fogDensity = previousFogDensity;
        RenderSettings.ambientLight = previousAmbientLight;
        RenderSettings.haloStrength = previousHaloStrength;
        RenderSettings.flareStrength = previousFlareStrength;
    }
}


