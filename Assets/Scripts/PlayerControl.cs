using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

    public float hitDistance;
    public bool IsGrounded;
    //Layer from which the player is able to jump from
    public LayerMask layer;

    public float jumpForce;

    public string playerPrefix;
    public Transform playerCamera;
    public Camera _playerCamera;
    public Canvas _playerCanvas;

    private float defaultSpeed;
    private float dashSpeed;
    private float pushingSpeed;
    private float shootingSpeed;
    private float dashDuration;
    private float lockAcquisitionRange;

    private ParticleSystem speedEffect;
    private GameObject bolt;
    private Rigidbody _rigidbody;
    private Transform lockOnArrow;
    private Renderer lockOnRend;
    private Color lockOnGreen;
    private Color lockOnRed;
    
    private float currentSpeed;
	private float dashTime;
	private float lastShot;
	private Transform lockOnTarget = null;
	private bool dash;
    private Vector3 dashDir;
    
	private bool lockOn;
	private bool grabbing;
    private bool seeThrough;
    private bool jumpAbility;
    private bool ability;

	void Start() {

        GameObject playerManagerGO = GameObject.Find("PlayerManager");
        PlayerManager playerManager = playerManagerGO.GetComponent<PlayerManager>();
        _rigidbody = GetComponent<Rigidbody>();
        defaultSpeed = playerManager.defaultSpeed;
        currentSpeed = defaultSpeed;
        dashSpeed = playerManager.dashSpeed;
        pushingSpeed = playerManager.pushingSpeed;
        shootingSpeed = playerManager.shootingSpeed;
        dashDuration = playerManager.dashDuration;
        lockAcquisitionRange = playerManager.lockAcquisitionRange;
        speedEffect = playerManager.speedEffect;
        bolt = playerManager.bolt;

        lockOnArrow = transform.Find("LockOnArrow");
        //lockOnRend = lockOnArrow.gameObject.GetComponent<Renderer>();
        //lockOnGreen = lockOnRend.material.color;
        lockOnRed = Color.red;
        
    }
    
    void Jump()
    {
        //TODO: Jump & Move with Character Controller
        _rigidbody.AddForce(Vector3.up * jumpForce);
        
    }

    //Manages the IsGrounded bool by raycasting down
    public void UpdateStats()
    {
        if (IsGrounded)
        {
            hitDistance =  0.35f;
        }else
        {
            hitDistance = 0.15f;
        }

        if(Physics.Raycast(transform.position - new Vector3(0,0.85f,0) ,-transform.up, hitDistance, layer)){
            IsGrounded = true;
        }else
        {
            IsGrounded = false;
        }
    }

    void Update() {
        
        UpdateStats();

        if (IsGrounded && Input.GetButtonDown(playerPrefix + "Item") && jumpAbility)
        {
            Jump();
        }

        float moveHorizontal = Input.GetAxis (playerPrefix + "Horizontal");
		float moveVertical = Input.GetAxis (playerPrefix + "Vertical");
		Vector3 movementPlayer = new Vector3 (moveHorizontal, 0, moveVertical);
        bool firstPerson = Input.GetButton(playerPrefix + "FirstPerson");


        if (Input.GetButtonDown (playerPrefix +  "Dash")) {
            dashDir = movementPlayer;
            dashDir = playerCamera.transform.TransformDirection(dashDir);
            dashDir.y = 0.0f;
            dash = true;
			currentSpeed = dashSpeed;
			var effect = Instantiate (speedEffect, transform.position, Quaternion.identity);
			effect.transform.parent = gameObject.transform;
		}

		if (dash) {
            movementPlayer = Vector3.zero;
			dashTime += Time.deltaTime;
			if (dashTime >= dashDuration) {
				dash = false;
				currentSpeed = defaultSpeed;
				dashTime = 0;
			}
		}
        
        if (seeThrough && Input.GetButton(playerPrefix + "Item"))
        {
            _playerCamera.cullingMask |= (1 << 8);
            
        }
        else
        {
            _playerCamera.cullingMask = ~(1 << 8);
        }
        
        if (grabbing && !Input.GetButton(playerPrefix + "Action")) {
			Transform go = transform.FindChild ("PushBlock");
			go.transform.parent = null;
			grabbing = false;
			currentSpeed = defaultSpeed;
		}

        if (movementPlayer != Vector3.zero) {
            movementPlayer = playerCamera.transform.TransformDirection(movementPlayer);
            movementPlayer.y = 0.0f;

            Quaternion rotation = new Quaternion(0, 0, playerCamera.rotation.z, 0);
            if (!lockOn)
            {
                transform.rotation = rotation;
                transform.rotation = Quaternion.LookRotation(movementPlayer);
            }
        }

        if (dash)
        {
            transform.Translate(dashDir * currentSpeed * Time.deltaTime, Space.World);

        }
        else
        {
            transform.Translate(movementPlayer * currentSpeed * Time.deltaTime, Space.World);
        }
        LockOnSystem();

		if (Input.GetButton(playerPrefix + "Shoot") && lastShot > shootingSpeed) {
			Instantiate(bolt, transform.position, transform.rotation);
			lastShot = 0;
		}
		lastShot += Time.deltaTime;
        
	}

    private void LockOnSystem()
    {
        lockOn = Input.GetButton(playerPrefix + "LockOn");

        // When not pressing lock on, scan nearby area for enemies. Display the lock on arrow on top of the closest enemy.
        if (!lockOn)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float closestDist = 0;
            Transform closestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float dist = Vector3.Distance(enemy.transform.position, transform.position);
                if (closestDist == 0)
                {
                    closestDist = dist;
                    closestEnemy = enemy.transform;
                }
                else if (closestDist > dist)
                {
                    closestDist = dist;
                    closestEnemy = enemy.transform;
                }
            }

            if (closestDist != 0 && closestDist < lockAcquisitionRange)
            {
                lockOnTarget = closestEnemy;
                Vector3 arrowPos = new Vector3(lockOnTarget.position.x, lockOnTarget.position.y + 1, lockOnTarget.position.z);
                lockOnArrow.gameObject.SetActive(true);
                lockOnArrow.transform.position = arrowPos;
            }
            else
            {
                lockOnArrow.gameObject.SetActive(false);
                lockOnTarget = null;
            }
        }
        // If pressing lock on, stop scanning for enemies and keep the arrow on the locked on enemy if there was one. 
        else 
        {
            transform.LookAt(lockOnTarget);
            if (lockOnTarget != null)
            {
                Vector3 arrowPos = new Vector3(lockOnTarget.position.x, lockOnTarget.position.y + 1, lockOnTarget.position.z);
                lockOnArrow.transform.position = arrowPos;
            }
            else
            {
                lockOnArrow.gameObject.SetActive(false);
            }
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
		if (other.tag == "SeeThrough" && !ability)
        {
            ability = true;
            seeThrough = true;
            _playerCanvas.GetComponent<UIManager>().EnableSeeThrough(true);
            
            //Destroy(other.gameObject);

        }

        if (other.tag == "Jump" && !ability)
        {
            ability = true;
            jumpAbility = true;
            _playerCanvas.GetComponent<UIManager>().EnableJump(true);

            //Destroy(other.gameObject);

        }
        if (other.tag == "Reset")
        {
            ability = false;
            jumpAbility = false;
            seeThrough = false;
            _playerCanvas.GetComponent<UIManager>().EnableJump(false);
            _playerCanvas.GetComponent<UIManager>().EnableSeeThrough(false);

        }
    }

}
