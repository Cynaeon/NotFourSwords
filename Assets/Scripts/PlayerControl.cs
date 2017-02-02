using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerControl : MonoBehaviour {

	public float defaultSpeed = 10.0f;
	public float dashSpeed = 30.0f;
	public float pushingSpeed = 5.0f;
	public float shootingSpeed = 0.5f;
	public float dashDuration = 0.5f;
	public Transform playerCamera;
	public ParticleSystem speedEffect;
	public GameObject bolt;
    public string playerPrefix;

    public Camera _playerCamera;
    public Canvas _playerCanvas;

    private float currentSpeed;
	private float dashTime;
	private float lastShot;
	private Transform lockOnTarget = null;
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
      
		lockOn = Input.GetButton (playerPrefix + "LockOn");
		if (lockOn) {
			if (lockOnTarget == null) {
				FindLockOnTarget ();
			} else {
				transform.LookAt (lockOnTarget);
			}
		} else {
			lockOnTarget = null;
		}

		if (Input.GetButton(playerPrefix + "Shoot") && lastShot > shootingSpeed) {
			Instantiate(bolt, transform.position, transform.rotation);
			lastShot = 0;
		}
		lastShot += Time.deltaTime;
	}


	private void FindLockOnTarget() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		float closestDist = 0;
		Transform closestEnemy = null;

		foreach (GameObject enemy in enemies) {
			float dist = Vector3.Distance (enemy.transform.position, transform.position);
			if (closestDist == 0) {
				closestDist = dist;
				closestEnemy = enemy.transform;
			} else if (closestDist > dist) {
				closestDist = dist;
				closestEnemy = enemy.transform;
			}
		}

		lockOnTarget = closestEnemy;
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
		if (other.tag == "SeeThrough") {
		    _playerCamera.cullingMask |= (1 << 8);

            _playerCanvas.GetComponent<UIManager>().EnableSeeThrough();

        }
	}

}
