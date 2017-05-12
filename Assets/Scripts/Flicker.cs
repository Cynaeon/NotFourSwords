using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour {
    public Light torchLight;
    public float minRange;
    public float maxRange;
    public float interval;

    private float time;
    private float newRange;

	void Awake () {
		
	}
	
	void Update () {
        time += Time.deltaTime;
        if (time > interval)
        {
            newRange = Random.Range(minRange, maxRange);
            torchLight.range = Mathf.Lerp(torchLight.range, newRange, Time.deltaTime * 10);
        }
	}
}
