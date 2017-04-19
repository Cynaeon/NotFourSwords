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
        float emission = Mathf.PingPong(Time.time * fadeSpeed, 0.5f) + 0.1f;
        Color baseColor = Color.cyan;

        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
        runes.SetColor("_EmissionColor", finalColor);
    }
}
