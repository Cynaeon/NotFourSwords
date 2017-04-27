using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lever : MonoBehaviour {

    public Transform handle;
    public Transform targetRot;
    public float rotateSpeed;
    public Text popupText;
    public int floorToUnlock;

    private bool activated;
    private GameObject gameManager;

	void Start () {
        gameManager = GameObject.Find("GameManager");
        popupText.enabled = false;
	}
	
	void Update () {
		if (activated)
        {
            handle.rotation = Quaternion.RotateTowards(handle.rotation, targetRot.rotation, Time.deltaTime * rotateSpeed);
        }
        if (handle.rotation == targetRot.rotation)
        {
            gameManager.GetComponent<GameManager>().floorsUnlocked[floorToUnlock] = true;
            popupText.enabled = true;
            popupText.text = "Floor " + floorToUnlock + " unlocked!";
        }
	}

    public void TurnLever()
    {
        activated = true;
    }
}
