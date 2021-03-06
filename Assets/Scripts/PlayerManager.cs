﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public int health = 9;
    public int score = 0;
    public float defaultSpeed = 10.0f;
    public float dashSpeed = 30.0f;
    public float pushingSpeed = 5.0f;
    public float shootingSpeed = 0.5f;
    public float burstSpeed = 0.2f;
    public float dashDuration = 0.5f;
    public float afterImageRatio = 0.1f;
    public float dashInvulTime = 0.2f;
    public float lockAcquisitionRange = 5.0f;
    public float lockMaxRange = 15.0f;
    public float gravity = 10f;
    public float jumpForce = 4f;
    public float dmgInvulTime = 2f;
    public float maxMagnetDistance = 50f;
    public float magnetVelocity = 25f;
    public float minMagnetDistance = 1f;
    public float deadzone = 0.25f;
    public float respawnTime = 5;

    //public Transform playerCamera;

    public GameObject player1;
	public GameObject player2;
	public GameObject player3;
	public GameObject player4;

    public List<Camera> mainCameras = new List<Camera>();

	void Start () {
		player1.SetActive (true);
		player2.SetActive (true);
		player3.SetActive (true);
		player4.SetActive (true);

		mainCameras[0].enabled = true;
		mainCameras[1].enabled = true;
        mainCameras[2].enabled = true;
        mainCameras[3].enabled = true;
    }

	void Update () {

        //SwitchPlayers();
        /*
        if (Input.GetButtonDown("P1_Start")) {
			if (!player1.activeSelf) {
                mainCameras[0].enabled = true;
				player1.SetActive (true);
			} else {
                mainCameras[0].enabled = false;
				player1.SetActive (false);
			}
		}
		if (Input.GetButtonDown("P2_Start")) {
			if (!player2.activeSelf) {
                mainCameras[1].enabled = true;
				player2.SetActive (true);
			} else {
                mainCameras[1].enabled = false;
				player2.SetActive (false);
			}
		}
		if (Input.GetButtonDown("P3_Start")) {
			if (!player3.activeSelf) {
                mainCameras[2].enabled = true;
				player3.SetActive (true);
			} else {
                mainCameras[2].enabled = false;
				player3.SetActive (false);
			}
		}
		if (Input.GetButtonDown("P4_Start")) {
			if (!player4.activeSelf) {
                mainCameras[3].enabled = true;
				player4.SetActive (true);
			} else {
                mainCameras[3].enabled = false;
				player4.SetActive (false);
			}
		}
        */

        OrganizeCameras(FindActiveCameras());
	}

    private void SwitchPlayers()
    {
        if (Input.GetAxis("P1_DPadVertical") > 0.1f)
        {
            Debug.Log("Switched to player1");
            player1.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.P1_;
            player2.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.NotSelected;
            player3.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.NotSelected;
            player4.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.NotSelected;
            mainCameras[0].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.P1_;
            mainCameras[1].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.NotSelected;
            mainCameras[2].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.NotSelected;
            mainCameras[3].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.NotSelected;
        }
        if (Input.GetAxis("P1_DPadHorizontal") < -0.1f)
        {
            Debug.Log("Switched to player2");
            player1.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.NotSelected;
            player2.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.P1_;
            player3.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.NotSelected;
            player4.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.NotSelected;
            mainCameras[0].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.NotSelected;
            mainCameras[1].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.P1_;
            mainCameras[2].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.NotSelected;
            mainCameras[3].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.NotSelected;
        }
        if (Input.GetAxis("P1_DPadHorizontal") > 0.1f)
        {
            Debug.Log("Switched to player3");
            player1.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.NotSelected;
            player2.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.NotSelected;
            player3.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.P1_;
            player4.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.NotSelected;
            mainCameras[0].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.NotSelected;
            mainCameras[1].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.NotSelected;
            mainCameras[2].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.P1_;
            mainCameras[3].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.NotSelected;
        }
        if (Input.GetAxis("P1_DPadVertical") < -0.1f)
        {
            Debug.Log("Switched to player4");
            player1.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.NotSelected;
            player2.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.NotSelected;
            player3.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.NotSelected;
            player4.GetComponent<PlayerControl>().playerPrefix = PlayerControl.Players.P1_;
            mainCameras[0].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.NotSelected;
            mainCameras[1].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.NotSelected;
            mainCameras[2].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.NotSelected;
            mainCameras[3].GetComponent<CameraControl>().playerPrefix = CameraControl.Players.P1_;
        }
    }

    private List<Camera> FindActiveCameras() {
        List<Camera> activeCams = new List<Camera>();

        foreach (Camera cam in mainCameras)
        {
            if (cam.enabled == true)
            {
                activeCams.Add(cam);
            }
        }
        return activeCams;
    }

    private void OrganizeCameras(List<Camera> activeCams) {


        if (activeCams.Count == 1)
        {
            foreach (Camera cam in activeCams)
            {
                if (activeCams.Count == 1)
                {
                    cam.rect = new Rect(0, 0, 1, 1);
                }
            }
        }
        else if (activeCams.Count == 2)
        {
            float xPos = 0;
            foreach (Camera cam in activeCams)
            {
                cam.rect = new Rect(xPos, 0, 0.5f, 1f);
                xPos += 0.5f;
            }
        }
        else if (activeCams.Count == 3)
        {
            int count = 0;
            foreach (Camera cam in activeCams)
            {
                if (count == 0)
                {
                    cam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                }
                else if (count == 1)
                {
                    cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                }
                else if (count == 2)
                {
                    cam.rect = new Rect(0, 0, 1f, 0.5f);
                }
                count++;
            }
        }
        else if (activeCams.Count == 4)
        {
            int count = 0;
            foreach (Camera cam in activeCams)
            {
                if (count == 0)
                {
                    cam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                }
                else if (count == 1)
                {
                    cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                }
                else if (count == 2)
                {
                    cam.rect = new Rect(0, 0, 0.5f, 0.5f);
                }
                else if (count == 3)
                {
                    cam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                }
                count++;
            }
        }
    }
}
