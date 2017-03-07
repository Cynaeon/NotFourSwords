﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public float health = 9.0f;
    public float defaultSpeed = 10.0f;
    public float dashSpeed = 30.0f;
    public float pushingSpeed = 5.0f;
    public float shootingSpeed = 0.5f;
    public float burstSpeed = 0.2f;
    public float dashDuration = 0.5f;
    public float afterImageRatio = 0.1f;
    public float invulTime = 0.2f;
    public float lockAcquisitionRange = 5.0f;
    public float lockMaxRange = 15.0f;
    public float gravity = 10f;
    public float jumpForce = 4f;

    public Transform playerCamera;
    public GameObject bolt;

    public GameObject player1;
	public GameObject player2;
	public GameObject player3;
	public GameObject player4;

    public List<Camera> mainCameras = new List<Camera>();

    public bool isPaused;
    private enum Pauser
    {
        P1,
        P2,
        P3,
        P4
    }
    private Pauser _pauser;

	void Start () {
		player1.SetActive (true);
		player2.SetActive (false);
		player3.SetActive (false);
		player4.SetActive (false);

		mainCameras[0].enabled = true;
		mainCameras[1].enabled = false;
        mainCameras[2].enabled = false;
        mainCameras[3].enabled = false;
    }

	void Update () {
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

        if (Input.GetButtonDown("P1_Pause"))
        {
            if (!isPaused)
            {
                isPaused = true;
                _pauser = Pauser.P1;
                Time.timeScale = 0;
            }
            else
            {
                if (_pauser == Pauser.P1)
                {
                    isPaused = false;
                    Time.timeScale = 1;
                }
            }
        }

        if (Input.GetButtonDown("P2_Pause"))
        {
            if (!isPaused)
            {
                isPaused = true;
                _pauser = Pauser.P2;
                Time.timeScale = 0;
            }
            else
            {
                if (_pauser == Pauser.P2)
                {
                    isPaused = false;
                    Time.timeScale = 1;
                }
            }
        }

        if (Input.GetButtonDown("P3_Pause"))
        {
            if (!isPaused)
            {
                isPaused = true;
                _pauser = Pauser.P3;
                Time.timeScale = 0;
            }
            else
            {
                if (_pauser == Pauser.P3)
                {
                    isPaused = false;
                    Time.timeScale = 1;
                }
            }
        }

        if (Input.GetButtonDown("P4_Pause"))
        {
            if (!isPaused)
            {
                isPaused = true;
                _pauser = Pauser.P4;
                Time.timeScale = 0;
            }
            else
            {
                if (_pauser == Pauser.P4)
                {
                    isPaused = false;
                    Time.timeScale = 1;
                }
            }
        }

        OrganizeCameras(FindActiveCameras());
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
