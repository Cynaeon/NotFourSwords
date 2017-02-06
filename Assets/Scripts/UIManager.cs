using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Image seeThroughImage;
    public Image jumpImage;
	// Use this for initialization
	void Start () {
        seeThroughImage.enabled = false;
        jumpImage.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void EnableSeeThrough(bool state)
    {
        seeThroughImage.enabled = state;
    }
    public void EnableJump(bool state)
    {
        jumpImage.enabled = state;
    }
}
