using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Image seeThroughImage;
	// Use this for initialization
	void Start () {
        seeThroughImage.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void EnableSeeThrough()
    {
        seeThroughImage.enabled = true;
    }
}
