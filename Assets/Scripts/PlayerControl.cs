using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	public float defaultSpeed = 10.0f;
	public float dashSpeed = 30.0f;
	public float pushingSpeed = 5.0f;
	public Transform playerCamera;
	public float dashDuration = 0.5f;
	public ParticleSystem speedEffect;
    public string playerPrefix;

	private float currentSpeed;
	private float dashTime;
	private bool dash;
	private bool lockOn;
	private bool grabbing;

	void Start() {
		currentSpeed = defaultSpeed;
	}

	void Update() {
		float moveHorizontal = Input.GetAxis (playerPrefix + "Horizontal");
		float moveVertical = Input.GetAxis (playerPrefix + "Vertical");
		Vector3 movementPlayer = new Vector3 (moveHorizontal, 0.0f, moveVertical);

        lockOn = Input.GetButton (playerPrefix + "LockOn");

		if (Input.GetButtonDown (playerPrefix +  "Dash")) {
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

	
		if(grabbing && !Input.GetButton(playerPrefix + "Action")) {
			Transform ga = transform.FindChild ("PushBlock");
			ga.transform.parent = null;
			grabbing = false;
			currentSpeed = defaultSpeed;
		}


        if (movementPlayer != Vector3.zero) {
            movementPlayer = playerCamera.transform.TransformDirection(movementPlayer);
            movementPlayer.y = 0.0f;

            Quaternion rotation = new Quaternion(0, 0, playerCamera.rotation.z, 0);
            if (!lockOn) {
                transform.rotation = rotation;
                transform.rotation = Quaternion.LookRotation(movementPlayer);
            }
        }
    
        transform.Translate(movementPlayer * currentSpeed * Time.deltaTime, Space.World);
      
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

    void OnTriggerStay (Collider other)
	{
		if (other.tag == "PushBlock") {
			if (Input.GetButton (playerPrefix + "Action")) {
				if (!grabbing) {
					other.transform.parent = this.gameObject.transform;
					grabbing = true;
				}
			}
		}
	}

}
