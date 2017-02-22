using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public float maxMagnetDistance;
    public float magnetVelocity;
    public float minMagnetDistance;
    private bool _magnetActive;

    public enum Items
    {
        none,
        jump,
        seeThrough,
        magnet
    }
    private Items myItem;

    #region Public Objects
    public string playerPrefix;
    public Transform playerCamera;
    public Camera _playerCamera;
    public Canvas _playerCanvas;
    public GameObject playerHitbox;
    public Texture2D crosshairTexture;
    public float crosshairScale = 1;
    #endregion

    #region Player Attributes
    private float health;
    private float defaultSpeed;
    private float dashSpeed;
    private float pushingSpeed;
    private float shootingSpeed;
    private float shootingLevel;
    private float dashDuration;
    private float invulTime;
    private float lockAcquisitionRange;
    private float lockMaxRange;
    private Collider _grabSpot;
    #endregion

    #region Other Objects
    private CharacterController controller;
    private ParticleSystem speedEffect;
    private GameObject bolt;
    private Transform lockOnArrow;
    private Renderer lockOnRend;
    private Color lockOnGreen;
    //private Color lockOnRed;
    private GameObject pushBlock;
    #endregion

    #region Private Variables
    private Vector3 movementPlayer;
    private float currentSpeed;
    private float verticalVelocity;
    private float gravity;
    private float jumpForce;
    private float dashTime;
    private float lastShot;
    private int burstCount;
    private float burstSpeed;
    private Transform lockOnTarget = null;
    private Vector3 dashDir;
    private int switchTarget;
    #endregion

    #region Private Booleans
    private bool burstShot;
    private bool lockOn;
    private bool firstPerson;
    private bool grabbing;
    private bool canSee;
    private bool canChangeItem;
    private bool canDash;
    private bool dash;
    #endregion

    void Start()
    {
        #region Get player attributes from manager
        GameObject playerManagerGO = GameObject.Find("PlayerManager");
        PlayerManager playerManager = playerManagerGO.GetComponent<PlayerManager>();
        controller = GetComponent<CharacterController>();
        health = playerManager.health;
        defaultSpeed = playerManager.defaultSpeed;
        currentSpeed = defaultSpeed;
        dashSpeed = playerManager.dashSpeed;
        pushingSpeed = playerManager.pushingSpeed;
        shootingSpeed = playerManager.shootingSpeed;
        burstSpeed = playerManager.burstSpeed;
        dashDuration = playerManager.dashDuration;
        invulTime = playerManager.invulTime;
        lockAcquisitionRange = playerManager.lockAcquisitionRange;
        lockMaxRange = playerManager.lockMaxRange;
        speedEffect = playerManager.speedEffect;
        bolt = playerManager.bolt;
        gravity = playerManager.gravity;
        jumpForce = playerManager.jumpForce;
        lockOnArrow = transform.Find("LockOnArrow");
        myItem = Items.none;

        _grabSpot = GetComponentInChildren<BoxCollider>();
        #endregion
    }

    void Update()
    {
        // ¯\_(ツ)_/¯
        GetMovement();
        Dashing();
        Gravity();
        Magnet();
        FirstPersonControls();
        Grabbing();
        Movement();
        LockOnSystem();
        Shooting();
        CheckDeath();
        Lens();
        SwitchItems();
    }

    private void GetMovement()
    {
        Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        float moveHorizontal = Input.GetAxis(playerPrefix + "Horizontal");
        float moveVertical = Input.GetAxis(playerPrefix + "Vertical");
        movementPlayer = (moveHorizontal * right + moveVertical * forward).normalized;
    }

    private void Movement()
    {
        if (movementPlayer != Vector3.zero)
        {
            Quaternion rotation = new Quaternion(0, 0, playerCamera.rotation.z, 0);
            if (!firstPerson && !lockOn && !grabbing && !_magnetActive)
            {
                transform.rotation = rotation;
                transform.rotation = Quaternion.LookRotation(movementPlayer);
            }
        }

        if (dash)
        {
            verticalVelocity = -gravity * Time.deltaTime;
            controller.Move(dashDir * currentSpeed * Time.deltaTime);
        }
        else
        {
            movementPlayer.y = verticalVelocity;
            controller.Move(movementPlayer * currentSpeed * Time.deltaTime);
        }
    }

    private void Grabbing()
    {
        if (Input.GetButton(playerPrefix + "Item"))
        {
            _grabSpot.enabled = false;
        } else
        {
            _grabSpot.enabled = true;
        }
        
        if (grabbing && !Input.GetButton(playerPrefix + "Action"))
        {
            pushBlock.GetComponent<PushBlock>().RemovePusher(gameObject);
            pushBlock = null;
            grabbing = false;
            currentSpeed = defaultSpeed;
        }

        if (grabbing && !firstPerson)
        {
            Vector3 direction = transform.position - pushBlock.transform.position;
            direction = direction.normalized;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                movementPlayer.z = 0.0f;

            }
            else
            {
                movementPlayer.x = 0.0f;
            }
            currentSpeed = pushingSpeed;
            movementPlayer.y = 0;
            bool canMove = pushBlock.GetComponent<PushBlock>().Move(movementPlayer, currentSpeed);
            if (!canMove)
            {
                movementPlayer = Vector3.zero;
            }
        }
        if (grabbing && !Input.GetButton(playerPrefix + "Action"))
        {
            grabbing = false;
            currentSpeed = defaultSpeed;
        }
    }

    private void Gravity()
    {
        if (controller.isGrounded)
        {
            canDash = true;
            verticalVelocity = -gravity * Time.deltaTime;
            if (Input.GetButtonDown(playerPrefix + "Item") && myItem == Items.jump)
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {

            verticalVelocity -= gravity * Time.deltaTime;
        }
    }

    private void SwitchItems()
    {

        if (canChangeItem)
        {
            int itemAvailable = FindClosestItemSpawner().gameObject.GetComponent<ItemSpawner>().checkActive();
            if(itemAvailable > 0 && itemAvailable != (int)myItem || itemAvailable == 0 && myItem != Items.none) 
            {
                _playerCanvas.GetComponent<UIManager>().EnableNotification(true);
            }
            if (Input.GetButtonDown(playerPrefix + "Action"))
            {
                FindClosestItemSpawner().gameObject.GetComponent<ItemSpawner>().changeActive((int)myItem);
                switch (itemAvailable)
                {
                    case 0:
                        myItem = Items.none;
                        _playerCanvas.GetComponent<UIManager>().EnableJump(false);
                        _playerCanvas.GetComponent<UIManager>().EnableSeeThrough(false);
                        _playerCanvas.GetComponent<UIManager>().EnableMagnet(false);
                        _playerCamera.cullingMask = ~(1 << 8);
                        canSee = false;
                        break;
                    case 1:
                        myItem = Items.jump;
                        _playerCanvas.GetComponent<UIManager>().EnableJump(true);
                        _playerCanvas.GetComponent<UIManager>().EnableSeeThrough(false);
                        _playerCanvas.GetComponent<UIManager>().EnableMagnet(false);
                        _playerCamera.cullingMask = ~(1 << 8);
                        canSee = false;
                        break;
                    case 2:
                        myItem = Items.seeThrough;
                        _playerCanvas.GetComponent<UIManager>().EnableJump(false);
                        _playerCanvas.GetComponent<UIManager>().EnableSeeThrough(true);
                        _playerCanvas.GetComponent<UIManager>().EnableMagnet(false);
                        break;
                    case 3:
                        myItem = Items.magnet;
                        _playerCanvas.GetComponent<UIManager>().EnableJump(false);
                        _playerCanvas.GetComponent<UIManager>().EnableSeeThrough(false);
                        _playerCanvas.GetComponent<UIManager>().EnableMagnet(true);
                        _playerCamera.cullingMask = ~(1 << 8);
                        canSee = false;
                        break;
                }
            }
        }else
        {
            _playerCanvas.GetComponent<UIManager>().EnableNotification(false);
        }
    }

    private void Lens()
    {
        if (Input.GetButtonDown(playerPrefix + "Item") && myItem == Items.seeThrough)
        {
            if (canSee)
            {
                _playerCamera.cullingMask = ~(1 << 8);
                _playerCamera.cullingMask |= (1 << 10);
                canSee = false;
            }
            else
            {
                _playerCamera.cullingMask |= (1 << 8);
                _playerCamera.cullingMask = ~(1 << 10);

                canSee = true;
            }
        }
    }

    private void CheckDeath()
    {
        if (transform.position.y < -15 || health <= 0)
        {
            transform.position = new Vector3(0, 2, 0);
            health = 10.0f;
        }
    }

    private void Magnet()
    {
        // Magnet
        // Works when no GrabSpot is present
        if (Input.GetButton(playerPrefix + "Item") && myItem == Items.magnet)
        {
            _magnetActive = true;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxMagnetDistance))
            {
                Debug.Log(hit.distance);
                if (hit.collider.tag == "Magnetic")
                {
                    // Sticking to objects will be added here
                    verticalVelocity = 0;
                    movementPlayer = Vector3.zero;
                    if (hit.distance >= minMagnetDistance)
                    {
                        controller.Move(transform.forward * Time.deltaTime * magnetVelocity);
                    }
                }
                else if (hit.collider.tag == "Metallic")
                {
                    // And magnetic lifting here
                    if (hit.distance >= minMagnetDistance)
                    {
                        hit.transform.Translate(-transform.forward * Time.deltaTime * magnetVelocity);
                    }
                }
            }
        } else
        {
            _magnetActive = false;
        }
    }

    private void Dashing()
    {
        if (movementPlayer != Vector3.zero && dashTime == 0 && Input.GetButtonDown(playerPrefix + "Dash"))
        {
            if (canDash)
            {
                dashDir = movementPlayer.normalized;
                //dashDir = playerCamera.transform.TransformDirection(dashDir);
                dashDir.y = 0.0f;
                dash = true;
                currentSpeed = dashSpeed;
                var effect = Instantiate(speedEffect, transform.position, Quaternion.identity);
                effect.transform.parent = gameObject.transform;
            }
        }

        if (dash)
        {
            if (invulTime >= dashTime)
            {
                playerHitbox.SetActive(false);
            }
            else
            {
                playerHitbox.SetActive(true);
            }
            grabbing = false;
            movementPlayer = Vector3.zero;
            dashTime += Time.deltaTime;

            if (dashTime >= dashDuration)
            {

                canDash = false;
                dash = false;
                currentSpeed = defaultSpeed;
                dashTime = 0;
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

    public void TakeDamage(float dmg)
    {
        health -= dmg;
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
            shootingSpeed = 0.3f;
            if (Input.GetButtonDown(playerPrefix + "Shoot") && burstShot == false && lastShot > shootingSpeed)
            {
                burstShot = true;
            }
            if (burstShot && lastShot > burstSpeed)
            {
                Instantiate(bolt, transform.position, transform.rotation);
                burstCount++;
                if (burstCount >= 3)
                {
                    burstShot = false;
                    burstCount = 0;
                }
                lastShot = 0;
            }
        }
        else if (shootingLevel >= 2)
        {
            shootingSpeed = 0.1f;
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
        if (Input.GetAxis(playerPrefix + "FirstPerson") > 0.5)
        {
            firstPerson = true;
        }
        else
        {
            firstPerson = false;
        }
        if (firstPerson && !_magnetActive)
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

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var enemyList = new List<GameObject>();

        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) < lockAcquisitionRange)
            {
                enemyList.Add(enemy);
            }
        }

        enemyList.Sort(delegate (GameObject a, GameObject b)
        {
            return Vector2.Distance(this.transform.position, a.transform.position)
            .CompareTo(
              Vector2.Distance(this.transform.position, b.transform.position));
        });

        if (!lockOn)
        {
            if (enemyList.Count > 0)
            {
                switchTarget = 0;
                if (Vector3.Distance(enemyList[0].transform.position, transform.position) < lockAcquisitionRange)
                {
                    lockOnTarget = enemyList[0].transform;
                    Vector3 arrowPos = new Vector3(lockOnTarget.position.x, lockOnTarget.position.y + 1.5f, lockOnTarget.position.z);
                    lockOnArrow.gameObject.SetActive(true);
                    lockOnArrow.transform.position = arrowPos;
                }
                else
                {
                    lockOnArrow.gameObject.SetActive(false);
                    lockOnTarget = null;
                }
            }
        }
        // Locked on
        else
        {
            if (lockOnTarget != null)
            {
                if (Vector3.Distance(lockOnTarget.transform.position, transform.position) <= lockMaxRange)
                {
                    transform.LookAt(lockOnTarget);
                    Vector3 arrowPos = new Vector3(lockOnTarget.position.x, lockOnTarget.position.y + 1.5f, lockOnTarget.position.z);
                    lockOnArrow.transform.position = arrowPos;
                }
                else
                {
                    lockOnTarget = null;
                    lockOnArrow.gameObject.SetActive(false);
                }
            }
            else
            {
                lockOnTarget = null;
                lockOnArrow.gameObject.SetActive(false);
            }

            if (Input.GetButtonDown(playerPrefix + "SwitchTarget"))
            {
                switchTarget++;
                if (switchTarget >= enemyList.Count)
                {
                    switchTarget = 0;
                }
                lockOnTarget = enemyList[switchTarget].transform;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "PushBlock")
        {
            if (Input.GetButton(playerPrefix + "Action"))
            {
                if (!grabbing)
                {
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

        if (other.tag == "SlidingIce")
        {
            
        }
    }

    public void ItemStateChange(bool changeTo)
    {
        canChangeItem = changeTo;
    }

    void OnTriggerExit(Collider other)
    {
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
