using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public GameObject player1;
	public GameObject player2;
	public GameObject player3;
	public GameObject player4;

	public Camera camera1;
	public Camera camera2;
	public Camera camera3;
	public Camera camera4;

	void Start () {
		player1.SetActive (true);
		player2.SetActive (false);
		player3.SetActive (false);
		player4.SetActive (false);

		camera1.enabled = true;
		camera2.enabled = false;
		camera3.enabled = false;
		camera4.enabled = false;
	}

	void Update () {
		if (Input.GetButtonDown("P1_Start")) {
			if (!player1.activeSelf) {
				camera1.enabled = true;
				player1.SetActive (true);
			} else {
				camera1.enabled = false;
				player1.SetActive (false);
			}
		}
		if (Input.GetButtonDown("P2_Start")) {
			if (!player2.activeSelf) {
				camera2.enabled = true;
				player2.SetActive (true);
			} else {
				camera2.enabled = false;
				player2.SetActive (false);
			}
		}
		if (Input.GetButtonDown("P3_Start")) {
			if (!player3.activeSelf) {
				camera3.enabled = true;
				player3.SetActive (true);
			} else {
				camera3.enabled = false;
				player3.SetActive (false);
			}
		}
		if (Input.GetButtonDown("P4_Start")) {
			if (!player4.activeSelf) {
				camera4.enabled = true;
				player4.SetActive (true);
			} else {
				camera4.enabled = false;
				player4.SetActive (false);
			}
		}

		int cameraCount = Camera.allCameras.Length;

		if (cameraCount == 1) {
			foreach (Camera cam in Camera.allCameras) {
				if (cameraCount == 1) {
					cam.rect = new Rect (0, 0, 1, 1);
				} 
			}
		} else if (cameraCount == 2) {
			float xPos = 0;
			foreach (Camera cam in Camera.allCameras) {
				cam.rect = new Rect (xPos, 0, 0.5f, 1f);
				xPos += 0.5f;
			}
		} else if (cameraCount == 3) {
			int count = 0;
			foreach (Camera cam in Camera.allCameras) {
				if (count == 0) {
					cam.rect = new Rect (0, 0.5f, 0.5f, 0.5f);
				} else if (count == 1) {
					cam.rect = new Rect (0.5f, 0.5f, 0.5f, 0.5f);
				} else if (count == 2) {
					cam.rect = new Rect (0, 0, 1f, 0.5f);
				}
				count++;
			}
		} else if (cameraCount == 4) {
			int count = 0;
			foreach (Camera cam in Camera.allCameras) {
				if (count == 0) {
					cam.rect = new Rect (0, 0.5f, 0.5f, 0.5f);
				} else if (count == 1) {
					cam.rect = new Rect (0.5f, 0.5f, 0.5f, 0.5f);
				} else if (count == 2) {
					cam.rect = new Rect (0, 0, 0.5f, 0.5f);
				} else if (count == 3) {
					cam.rect = new Rect (0.5f, 0, 0.5f, 0.5f);
				}
				count++;
			}
		}
	}
}
