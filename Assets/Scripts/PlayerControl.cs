using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

    public CharacterController controller;
    private float verticalVelocity;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpForce;

    private int _myItem;
    enum Items
    {
        none,
        jump,
        seeThrough
    }

    public string playerPrefix;
    public Transform playerCamera;
    public Camera _playerCamera;
    public Canvas _playerCanvas;

    public Texture2D crosshairTexture;
    public float crosshairScale = 1;

    private float defaultSpeed;
    private float dashSpeed;
    private float pushingSpeed;
    private float shootingSpeed;
    private float shootingLevel;
    private float dashDuration;
    private float lockAcquisitionRange;

    private ParticleSystem speedEffect;
    private GameObject bolt;
    private Transform lockOnArrow;
    private Renderer lockOnRend;
    private Color lockOnGreen;
    private Color lockOnRed;
    private GameObject pushBlock;
    
    private float currentSpeed;
	private float dashTime;
	private float lastShot;
	private Transform lockOnTarget = null;
	private bool dash;
    private Vector3 dashDir;
    
	private bool lockOn;
    private bool firstPerson;
	private bool grabbing;
    private bool ability;
    private bool canSee;
    private bool canChangeItem;

	void Start() {

        GameObject playerManagerGO = GameObject.Find("PlayerManager");
        PlayerManager playerManager = playerManagerGO.GetComponent<PlayerManager>();
        controller = GetComponent<CharacterController>();
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
        
        gravity = 10f;
        jumpForce = 4f;
        _myItem = (int)Items.none;
        canSee = false;
    }

    void Update() {

        float moveHorizontal = Input.GetAxis (playerPrefix + "Horizontal");
		float moveVertical = Input.GetAxis (playerPrefix + "Vertical");
		Vector3 movementPlayer = new Vector3 (moveHorizontal, 0 , moveVertical);
            
        if (dashTime == 0 && Input.GetButtonDown (playerPrefix +  "Dash") && controller.isGrounded) {
            dashDir = movementPlayer;
            dashDir = playerCamera.transform.TransformDirection(dashDir);
            dashDir.y = 0.0f;
            dash = true;
			currentSpeed = dashSpeed;
			var effect = Instantiate (speedEffect, transform.position, Quaternion.identity);
			effect.transform.parent = gameObject.transform;
		}

		if (dash) {
            grabbing = false;
            movementPlayer = Vector3.zero;
			dashTime += Time.deltaTime;
			if (dashTime >= dashDuration) {
				dash = false;
				currentSpeed = defaultSpeed;
				dashTime = 0;
			}
		}
        
        

        if (grabbing && !Input.GetButton(playerPrefix + "Action")) {
            pushBlock.GetComponent<PushBlock>().RemovePusher(gameObject);
            pushBlock = null;
			grabbing = false;
			currentSpeed = defaultSpeed;

		}


        if (movementPlayer != Vector3.zero) {
            movementPlayer = playerCamera.transform.TransformDirection(movementPlayer);
            movementPlayer.y = 0.0f;

            Quaternion rotation = new Quaternion(0, 0, playerCamera.rotation.z, 0);
            if (!firstPerson && !lockOn && !grabbing)
            {
                transform.rotation = rotation;
                transform.rotation = Quaternion.LookRotation(movementPlayer);
            }
        }

        //gravity
        if (controller.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;
            if (Input.GetButtonDown(playerPrefix + "Item") && _myItem == (int)Items.jump)
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        //movement
        if (dash)
        {
            //transform.Translate(dashDir * currentSpeed * Time.deltaTime, Space.World);
            movementPlayer.y = verticalVelocity;
            controller.Move(dashDir * currentSpeed * Time.deltaTime);
            
        }
        else
        {
            // transform.Translate(movementPlayer * currentSpeed * Time.deltaTime, Space.World);
            movementPlayer.y = verticalVelocity;
            controller.Move(movementPlayer * currentSpeed * Time.deltaTime);
        }
        
        if (Input.GetAxis(playerPrefix + "FirstPerson") > 0.5)
        {
            firstPerson = true;
            FirstPersonControls();
        }
        else
        {

            if (grabbing)
            {
                if (Mathf.Abs(movementPlayer.x) > Mathf.Abs(movementPlayer.z))
                {
                    movementPlayer.z = 0.0f;
                }
                else
                {
                    movementPlayer.x = 0.0f;
                }
                pushBlock.GetComponent<PushBlock>().Move(movementPlayer, currentSpeed);
            }
            
            transform.Translate(movementPlayer * currentSpeed * Time.deltaTime, Space.World);

            firstPerson = false;

        }

        LockOnSystem();
        Shooting();

        if (transform.position.y < -15)
        {
            transform.position = new Vector3(0, 2, 0);
        }
        
        if (_myItem == (int) Items.seeThrough  && Input.GetButtonUp(playerPrefix + "Item"))
        {
            if (canSee)
            {
                _playerCamera.cullingMask = ~(1 << 8);
                canSee = false;
            }
            else
            {
                _playerCamera.cullingMask |= (1 << 8);
                canSee = true;
            }
        }
        
        if (grabbing && !Input.GetButton(playerPrefix + "Action"))
        {
            Transform go = transform.FindChild("PushBlock");
            go.transform.parent = null;
            grabbing = false;
            currentSpeed = defaultSpeed;
        }


        if (Input.GetButtonDown(playerPrefix + "Action"))
        {
            if (canChangeItem)
            {
                int itemAvailable = FindClosestItemSpawner().gameObject.GetComponent<ItemSpawner>().checkActive();
                FindClosestItemSpawner().gameObject.GetComponent<ItemSpawner>().changeActive(_myItem);
                switch (itemAvailable)
                {
                    case 0:
                        {
                            _myItem = (int)Items.none;
                            _playerCanvas.GetComponent<UIManager>().EnableJump(false);
                            _playerCanvas.GetComponent<UIManager>().EnableSeeThrough(false);
                            break;
                        }
                    case 1:
                        {
                            _myItem = (int)Items.jump;
                            _playerCanvas.GetComponent<UIManager>().EnableJump(true);
                            _playerCanvas.GetComponent<UIManager>().EnableSeeThrough(false);
                            _playerCamera.cullingMask = ~(1 << 8);
                            canSee = false;
                            break;
                        }
                    case 2:
                        {
                            _myItem = (int)Items.seeThrough;
                            _playerCanvas.GetComponent<UIManager>().EnableJump(false);
                            _playerCanvas.GetComponent<UIManager>().EnableSeeThrough(true);
                            break;
                        }
                }
            }
        }
    }

    private GameObject FindClosestItemSpawner()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("ItemSpawner");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }


    private void Shooting()
    {
        if (shootingLevel == 0)
        {
            shootingSpeed = 0.5f;
            if (Input.GetButtonDown(playerPrefix + "Shoot") && lastShot > shootingSpeed)
            {
                Instantiate(bolt, transform.position, transform.rotation);
                lastShot = 0;
            }
        }
        else if (shootingLevel == 1)
        {
            shootingSpeed = 0.01f;
            if (Input.GetButtonDown(playerPrefix + "Shoot"))
            {
                Instantiate(bolt, transform.position, transform.rotation);
                lastShot = 0;
            }
        }
        else if (shootingLevel >= 2)
        {
            shootingSpeed = 0.05f;
            if (Input.GetButton(playerPrefix + "Shoot") && lastShot > shootingSpeed)
            {
                Instantiate(bolt, transform.position, transform.rotation);
                lastShot = 0;
            }
        }
        lastShot += Time.deltaTime;
    }

    private void FirstPersonControls()
    {
        if (firstPerson)
        {
            float lookHorizontal = Input.GetAxis(playerPrefix + "HorizontalRightStick");
            float lookVertical = Input.GetAxis(playerPrefix + "VerticalRightStick");
            Vector3 lookPlayer = new Vector3(-lookVertical, -lookHorizontal, 0);

            transform.localEulerAngles += lookPlayer;
        }
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
                    pushBlock = other.gameObject;
                    pushBlock.GetComponent<PushBlock>().AddPusher(gameObject);
					grabbing = true;
				}
			}
		}

    if (other.tag == "PowerUp")
        {
            shootingLevel++;
            Destroy(other.gameObject);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ItemSpawner")
        {
            canChangeItem = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "ItemSpawner")
        {
            canChangeItem = false;
        }
        if (other.tag == "PushBlock")
        {
            if (grabbing)
            {
                pushBlock.GetComponent<PushBlock>().RemovePusher(gameObject);
                pushBlock = null;
                grabbing = false;
                currentSpeed = defaultSpeed;
            }
        }
    }

    void OnGUI()
    {
        if (firstPerson)
        {
            GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width * crosshairScale) / 2, (Screen.height - crosshairTexture.height * crosshairScale) / 2, crosshairTexture.width * crosshairScale, crosshairTexture.height * crosshairScale), crosshairTexture);
        }
    }
}
