using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGlow : MonoBehaviour {

    public Material runes;
    public float fadeSpeed;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        float emission = Mathf.PingPong(Time.time * fadeSpeed, 0.7f) + 0.05f;
        Color baseColor = runes.color;

        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
        runes.SetColor("_EmissionColor", finalColor);
    }
}
