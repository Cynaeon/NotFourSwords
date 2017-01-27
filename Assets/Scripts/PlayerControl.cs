using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	public float speed = 10.0F;
	public Transform playerCamera;

	private bool lockOn;

	void Update() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

		if (movement != Vector3.zero) {
			movement = playerCamera.transform.TransformDirection (movement);
			movement.y = 0.0f;

			Quaternion rotation = new Quaternion (0, 0, playerCamera.rotation.z, 0);
			transform.rotation = rotation;
			transform.rotation = Quaternion.LookRotation (movement);

		}
		transform.Translate (movement * speed * Time.deltaTime, Space.World);

		lockOn = Input.GetButton ("LockOn");
		if (lockOn) {
			GameObject enemy = GameObject.FindGameObjectWithTag ("Enemy");
			transform.LookAt (enemy.transform);
		}
	}
}
