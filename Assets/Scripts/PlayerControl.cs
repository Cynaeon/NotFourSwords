using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	public float defaultSpeed = 10.0f;
	public float dashSpeed = 30.0f;
	public Transform playerCamera;
	public float dashDuration = 0.5f;
	public ParticleSystem speedEffect;

	private float currentSpeed;
	private float dashTime;
	private bool dash;
	private bool lockOn;

	void Start() {
		currentSpeed = defaultSpeed;
	}

	void Update() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

		lockOn = Input.GetButton ("LockOn");

		if (Input.GetButtonDown ("Dash")) {
			dash = true;
			currentSpeed = dashSpeed;
			var effect = Instantiate (speedEffect, transform.position, Quaternion.identity);
			effect.transform.parent = gameObject.transform;
		}

		if (dash) {
			dashTime += Time.deltaTime;
			if (dashTime >= dashDuration) {
				dash = false;
				currentSpeed = defaultSpeed;
				dashTime = 0;
			}
		}


		if (movement != Vector3.zero) {
			movement = playerCamera.transform.TransformDirection (movement);
			movement.y = 0.0f;

			Quaternion rotation = new Quaternion (0, 0, playerCamera.rotation.z, 0);
			if (!lockOn) {
				transform.rotation = rotation;
				transform.rotation = Quaternion.LookRotation (movement);
			}

		}

		transform.Translate (movement * currentSpeed * Time.deltaTime, Space.World);

		if (lockOn) {
			LockOnEnemy ();
		}
	}


	private void LockOnEnemy() {
		GameObject enemy = GameObject.FindGameObjectWithTag ("Enemy");
		if (enemy != null) {
			transform.LookAt (enemy.transform);
		} else {

		}
	}
}
