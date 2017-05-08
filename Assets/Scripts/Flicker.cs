using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour {
    public Light torchLight;
    public float minRange;
    public float maxRange;

	void Awake () {
		
	}
	
	void Update () {
        torchLight.range = Random.Range(minRange, maxRange);
	}
}
