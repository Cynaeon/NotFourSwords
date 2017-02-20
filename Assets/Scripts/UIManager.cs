using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Image seeThroughImage;
    public Image jumpImage;
    public Image magnetImage;
    public Image notificationImage;
	// Use this for initialization
	void Start () {
        seeThroughImage.enabled = false;
        jumpImage.enabled = false;
        magnetImage.enabled = false;
        notificationImage.enabled = false;
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
    public void EnableMagnet(bool state)
    {
        magnetImage.enabled = state;
    }
    public void EnableNotification(bool state)
    {
        notificationImage.enabled = state;
    }
}
