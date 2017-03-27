﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public enum Players
    {
        NotSelected,
        P1_,
        P2_,
        P3_,
        P4_
    }
    public Players playerPrefix;

    public PauseManager pauseManager;
    private bool _isPaused;
    public Transform startPos;

    public enum Items
    {
        none,
        jump,
        seeThrough,
        magnet,
        sword
    }
    private Items myItem;

    public enum StateOfTheAnimation
    {
        idle,
        idle_Sword,
        running,
        running_Sword,
        dashing

    }
    private StateOfTheAnimation activeState;
    private Animator anime;

    #region Public Objects
    public Transform playerCamera;
    public Camera _playerCamera;
    public Canvas _playerCanvas;
    public GameObject playerHitbox;
    public GameObject trailModel;
    public GameObject playerModel;
    public GameObject SwordItem;
    public GameObject Sheath;
    public GameObject SheathedSword;
    public GameObject MonocleItem;
    public GameObject MagnetItem;
    public GameObject BootsItem;
    public GameObject SwordHitBox;
    #endregion

    #region Player Attributes
    [HideInInspector] public float maxHealth;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public int gatheredCoins;
    private float defaultSpeed;
    private float dashSpeed;
    private float pushingSpeed;
    private float shootingSpeed;
    private float shootingLevel;
    private float dashDuration;
    private float afterImageRatio;
    private float dashInvulTime;
    private float lockAcquisitionRange;
    private float lockMaxRange;
    private float _maxMagnetDistance;
    private float _magnetVelocity;
    private float _minMagnetDistance;
    private float dmgInvulTime;
    private Collider _grabSpot;
    #endregion

    #region Other Objects
    private CharacterController controller;
    private GameObject bolt;
    private Transform lockOnArrow;
    private Renderer lockOnRend;
    private Color lockOnGreen;
    private Color lockOnRed;
    private GameObject pushBlock;
    private Renderer _rend;
    #endregion

    #region Private Variables
    private GameObject gameManager;
    private PlayerManager playerManager;
    private Vector3 movementPlayer;
    private float currentSpeed;
    private float verticalVelocity;
    private float gravity;
    private float jumpForce;
    private float dashTime;
    private float afterImageTime;
    private float lastShot;
    private float currentInvulTime;
    private int burstCount;
    //private float burstSpeed;
    private Transform lockOnTarget = null;
    private Vector3 dashDir;
    private int switchTarget;
    private Color defaultColor;
    #endregion

    #region Private Booleans
    private bool burstShot;
    private bool lockOn;
    [HideInInspector] public bool firstPerson;
    private bool grabbing;
    private bool climbing;
    private bool canSee;
    private bool canChangeItem;
    private bool canDash;
    private bool dash;
    private bool toggleSword;
    private bool invulnerable;
    private bool _magnetActive;
    [HideInInspector] public bool settingStartPos;
    private FogDensity fogDensity;
    #endregion

    void Start()
    {
        #region Get player attributes from manager
        GameObject playerManagerGO = GameObject.Find("PlayerManager");
        pauseManager = GetComponent<PauseManager>();
        playerManager = playerManagerGO.GetComponent<PlayerManager>();
        fogDensity = _playerCamera.GetComponent<FogDensity>();
        anime = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        currentHealth = playerManager.health;
        gatheredCoins = playerManager.coins;
        maxHealth = playerManager.health;
        defaultSpeed = playerManager.defaultSpeed;
        currentSpeed = defaultSpeed;
        dashSpeed = playerManager.dashSpeed;
        pushingSpeed = playerManager.pushingSpeed;
        shootingSpeed = playerManager.shootingSpeed;
        //burstSpeed = playerManager.burstSpeed;
        dashDuration = playerManager.dashDuration;
        afterImageRatio = playerManager.afterImageRatio;
        dashInvulTime = playerManager.dashInvulTime;
        dmgInvulTime = playerManager.dmgInvulTime;
        lockAcquisitionRange = playerManager.lockAcquisitionRange;
        lockMaxRange = playerManager.lockMaxRange;
        bolt = playerManager.bolt;
        gravity = playerManager.gravity;
        jumpForce = playerManager.jumpForce;
        lockOnArrow = transform.Find("LockOnArrow");
        lockOnGreen = lockOnArrow.GetComponent<Renderer>().material.color;
        lockOnRed = Color.red;
        myItem = Items.none;
        activeState = StateOfTheAnimation.idle;
        _maxMagnetDistance = playerManager.maxMagnetDistance;
        _magnetVelocity = playerManager.magnetVelocity;
        _minMagnetDistance = playerManager.minMagnetDistance;
        _grabSpot = GetComponentInChildren<BoxCollider>();
        #endregion

        gameManager = GameObject.Find("GameManager");
        SetActivity(false, false, false, false, false, false);
        _rend = GetComponentInChildren<SkinnedMeshRenderer>();
        defaultColor = _rend.material.color;
        settingStartPos = true;
    }

    void Update()
    {
        // ¯\_(ツ)_/¯

        _isPaused = pauseManager.isPaused;
        if (!_isPaused)
        {
            Gravity();

            if (playerPrefix != Players.NotSelected && !gameManager.GetComponent<GameManager>().disableMovement)
            {
                GetMovement();
                Dashing();
                Magnet();
                FirstPersonControls();
                Grabbing();
                Movement();
                LockOnSystem();
                Shooting();
                Health();
                SwitchItems();
                Swording();
                Lens();
                Animations();
            }
        }
    }

    public void SetStartPosition(Transform startPos)
    {
        this.startPos = startPos;
        transform.rotation = startPos.rotation;
        if ((int)playerPrefix == 0)
        {
            transform.position = new Vector3(startPos.position.x, startPos.position.y, startPos.position.z);
        }
        if ((int)playerPrefix == 1)
        {
            transform.position = new Vector3(startPos.position.x - 2, startPos.position.y, startPos.position.z + 2);
        }
        if ((int)playerPrefix == 2)
        {
            transform.position = new Vector3(startPos.position.x + 2, startPos.position.y, startPos.position.z + 2);
        }
        if ((int)playerPrefix == 3)
        {
            transform.position = new Vector3(startPos.position.x - 2, startPos.position.y, startPos.position.z - 2);
        }
        if ((int)playerPrefix == 4)
        {
            transform.position = new Vector3(startPos.position.x + 2, startPos.position.y, startPos.position.z - 2);
        }
        settingStartPos = false;
            
    }
    

    private void GetMovement()
    {
        Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        float moveHorizontal = Input.GetAxis(playerPrefix + "Horizontal");
        float moveVertical = Input.GetAxis(playerPrefix + "Vertical");
        if (moveHorizontal < 0.3 && moveHorizontal > -0.3)
        {
            moveHorizontal = 0;
        }
        if (moveVertical < 0.3 && moveVertical > -0.3)
        {
            moveVertical = 0;
        }
        movementPlayer = (moveHorizontal * right + moveVertical * forward).normalized;
    }

    private void Movement()
    {
        if (movementPlayer != Vector3.zero)
        {
            Quaternion rotation = new Quaternion(0, 0, playerCamera.rotation.z, 0);
            // Put a boolean in the if-statement below if you don't want the player to rotate
            if (!firstPerson && !lockOn && !grabbing && !_magnetActive && !climbing)
            {
                transform.rotation = rotation;
                transform.rotation = Quaternion.LookRotation(movementPlayer);
            }
        }

        if (dash)
        {
            verticalVelocity = -gravity * Time.deltaTime;
            controller.Move(dashDir * currentSpeed * Time.deltaTime);
            // Create cool after images
            afterImageTime -= Time.deltaTime;
            if (afterImageTime <= 0)
            {
                //Quaternion rot = playerModel.transform.rotation;
                Vector3 pos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
                Instantiate(trailModel, pos, transform.rotation);
                afterImageTime = afterImageRatio;
            }
        }
        else if (climbing)
        {
            float moveVertical = Input.GetAxis(playerPrefix + "Vertical");
            movementPlayer = new Vector3(0, moveVertical, 0);
            controller.Move(movementPlayer * currentSpeed * Time.deltaTime);
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
            if (Input.GetButton(playerPrefix + "Item") && myItem == Items.jump)
            {
                verticalVelocity = jumpForce;
            }
        }
        else if (!climbing)
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
                        _playerCanvas.GetComponent<UIManager>().UIItems(false, false, false, false);
                        SetActivity(false,false, false, false, false, false);
                        fogDensity.fadeState(false);
                        toggleSword = false;
                        _playerCamera.cullingMask = ~(1 << 8);
                        canSee = false;
                        break;
                    case 1:
                        myItem = Items.jump;
                        _playerCanvas.GetComponent<UIManager>().UIItems(true, false, false, false);
                        fogDensity.fadeState(false);
                        SetActivity(false,false, false, false, false, true);
                        toggleSword = false;
                        _playerCamera.cullingMask = ~(1 << 8);
                        canSee = false;
                        break;
                    case 2:
                        myItem = Items.seeThrough;
                        _playerCanvas.GetComponent<UIManager>().UIItems(false, true, false, false);
                        SetActivity(false,false, false, true, false, false);
                        toggleSword = false;
                        canSee = true;
                        break;
                    case 3:
                        myItem = Items.magnet;
                        _playerCanvas.GetComponent<UIManager>().UIItems(false, false, true, false);
                        SetActivity(false,false, false, false, true, false);
                        fogDensity.fadeState(false);
                        _playerCamera.cullingMask = ~(1 << 8);
                        toggleSword = false;
                        canSee = false;
                        break;
                    case 4:
                        myItem = Items.sword;
                        _playerCanvas.GetComponent<UIManager>().UIItems(false, false, false, true);
                        SetActivity(false,true, true, false, false, false);
                        fogDensity.fadeState(false);
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

    private void SetActivity(bool _sword, bool _sheath, bool _sheathedSword, bool _monocle, bool _magnet, bool _boots)
    {
        SwordItem.SetActive(_sword);
        Sheath.SetActive(_sheath);

        SheathedSword.SetActive(_sheathedSword);
        MonocleItem.SetActive(_monocle);
        MagnetItem.SetActive(_magnet);
        BootsItem.SetActive(_boots);
    }

    private void Lens()
    {
        if (Input.GetButtonDown(playerPrefix + "Item") && myItem == Items.seeThrough)
        {
            if (canSee)
            {
                fogDensity.fadeState(true);
                _playerCamera.clearFlags = CameraClearFlags.SolidColor;
                _playerCamera.cullingMask |= (1 << 8);
                _playerCamera.cullingMask = ~(1 << 10);
                canSee = false;
            }
            else
            {
                _playerCamera.clearFlags = CameraClearFlags.SolidColor;
                fogDensity.fadeState(false);
                _playerCamera.cullingMask = ~(1 << 8);
                _playerCamera.cullingMask |= (1 << 10);
                canSee = true;
            }
        }
    }

    private void Swording()
    {
        if (Input.GetButtonDown(playerPrefix + "Item") && myItem == Items.sword)
        {
            if (!toggleSword)
            {
                SwordItem.SetActive(true);
                SheathedSword.SetActive(false);
                toggleSword = true;
            }else
            {
                toggleSword = false;
                SwordItem.SetActive(false);
                SheathedSword.SetActive(true);
            }
            //TODO: Play sword animation
            
        }
    }

    private void Health()
    {
        // When invulnerable after taking dmg, flash red and disable hitbox
        if (invulnerable && currentInvulTime <= dmgInvulTime)
        {
            playerHitbox.SetActive(false);
            _rend.material.color = Color.Lerp(defaultColor, Color.red, Mathf.PingPong(Time.time, 0.2f));
            currentInvulTime += Time.deltaTime;
        }
        else
        {
            playerHitbox.SetActive(true);
            _rend.material.color = defaultColor;
            currentInvulTime = 0;
            invulnerable = false;
        }

        // Check death
        if (transform.position.y < -25 || currentHealth <= 0)
        {
            if (startPos)
            {
                transform.position = startPos.position;
                transform.rotation = startPos.rotation;
            }
            else
            {
                transform.position = Vector3.zero;
            }
            currentHealth = 10.0f;
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
            if (Physics.Raycast(transform.position, transform.forward, out hit, _maxMagnetDistance))
            {
                Debug.Log(hit.distance);
                if (hit.collider.tag == "Magnetic")
                {
                    // Sticking to objects will be added here
                    verticalVelocity = 0;
                    movementPlayer = Vector3.zero;
                    if (hit.distance >= _minMagnetDistance)
                    {
                        controller.Move(transform.forward * Time.deltaTime * _magnetVelocity);
                    }
                }
                else if (hit.collider.tag == "Metallic")
                {
                    // And magnetic lifting here
                    if (hit.distance >= _minMagnetDistance)
                    {
                        hit.transform.Translate(-transform.forward * Time.deltaTime * _magnetVelocity);
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
            }
        }

        if (dash)
        {
            if (dashInvulTime >= dashTime)
            {
                playerHitbox.SetActive(false);
            }
            else
            {
                playerHitbox.SetActive(true);
            }
            activeState = StateOfTheAnimation.dashing;
            grabbing = false;
            movementPlayer = Vector3.zero;
            dashTime += Time.deltaTime;

            if (dashTime >= dashDuration)
            {
                activeState = StateOfTheAnimation.idle;
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

    public void TakeDamage(float dmg, Vector3 dir)
    {
        invulnerable = true;
        gameObject.GetComponent<ImpactReceiver>().AddImpact(Vector3.back + Vector3.up, 100);
        movementPlayer = -movementPlayer;
        currentHealth -= dmg;
    }

    public void HealDamage(float heal)
    {
        if (currentHealth != maxHealth)
        {
            currentHealth += heal;
        }
    }

    public void IncreaseScore(int amount)
    {
        gatheredCoins = gatheredCoins + amount;
    }

    public void IncreaseShootingLevel(int upgrade)
    {
        shootingLevel += upgrade;
    }

    private void Shooting()
    {
        if (!dash && !toggleSword) {
            if (shootingLevel == 0)
            {
                shootingSpeed = 0.5f;
                if (Input.GetButtonDown(playerPrefix + "Shoot") && lastShot > shootingSpeed)
                {
                    Vector3 pos = new Vector3(transform.position.x , transform.position.y + .6f, transform.position.z);
                    Instantiate(bolt, pos, transform.rotation);
                    lastShot = 0;
                }
            }
            else if (shootingLevel == 1)
            {
                shootingSpeed = 0.3f;
                if (Input.GetButtonDown(playerPrefix + "Shoot") && lastShot > shootingSpeed)
                {
                    Vector3 pos = new Vector3(transform.position.x, transform.position.y + .6f, transform.position.z);
                    Instantiate(bolt, pos, transform.rotation);
                    lastShot = 0;
                }

                /*if (Input.GetButtonDown(playerPrefix + "Shoot") && burstShot == false && lastShot > shootingSpeed)
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
                }*/
            }
            else if (shootingLevel >= 2)
            {
                shootingSpeed = 0.1f;
                if (Input.GetButton(playerPrefix + "Shoot") && lastShot > shootingSpeed)
                {
                    Vector3 pos = new Vector3(transform.position.x, transform.position.y + .6f, transform.position.z);
                    Instantiate(bolt, pos, transform.rotation);
                    lastShot = 0;
                }
            }
            lastShot += Time.deltaTime;
        }
        if (toggleSword)
        {
            if(Input.GetButtonDown(playerPrefix + "Shoot")) {
                SwordHitBox.SetActive(true);
            }
        }
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
            if (playerPrefix == Players.P1_)
            {
                _playerCamera.cullingMask = ~(1 << 13);
            }
            else if (playerPrefix == Players.P2_)
            {
                _playerCamera.cullingMask = ~(1 << 14);
            }
            else if (playerPrefix == Players.P3_)
            {
                _playerCamera.cullingMask = ~(1 << 15);
            }
            else if (playerPrefix == Players.P4_)
            {
                _playerCamera.cullingMask = ~(1 << 16);
            }
            float lookHorizontal = Input.GetAxis(playerPrefix + "HorizontalRightStick");
            float lookVertical = Input.GetAxis(playerPrefix + "VerticalRightStick");
            Vector3 lookPlayer = new Vector3(-lookVertical, -lookHorizontal, 0);

            transform.localEulerAngles += lookPlayer;
        }
        else
        {
            if (playerPrefix == Players.P1_)
            {
                _playerCamera.cullingMask |= (1 << 13);
            }
            else if (playerPrefix == Players.P2_)
            {
                _playerCamera.cullingMask |= (1 << 14);
            }
            else if (playerPrefix == Players.P3_)
            {
                _playerCamera.cullingMask |= (1 << 15);
            }
            else if (playerPrefix == Players.P4_)
            {
                _playerCamera.cullingMask |= (1 << 16);
            }
            
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
            lockOnArrow.GetComponent<Renderer>().material.color = lockOnGreen;
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
            else
            {
                lockOnArrow.gameObject.SetActive(false);
                lockOnTarget = null;
            }
        }
        // Locked on
        else
        {
            lockOnArrow.GetComponent<Renderer>().material.color = lockOnRed;
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

        if (other.tag == "Ladder")
        {
            if (Input.GetButtonDown(playerPrefix + "Action"))
            {
                if (!climbing)
                {
                    climbing = true;
                } else
                {
                    climbing = false;
                }
            }
        }

        if (other.tag == "Pot")
        {
            if (dash)
            {
                other.GetComponent<Pot>().Break();
            }
        }

        if (other.tag == "SlidingIce")
        {
            // xD
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

        if (other.tag == "Ladder")
        {
            if (climbing)
            {
                climbing = false;
            }
        }
    }

    public void Animations()
    {
        float myAngle = Mathf.Atan2(Input.GetAxis(playerPrefix + "Horizontal"), Input.GetAxis(playerPrefix + "Vertical")) * Mathf.Rad2Deg;
        if (lockOn && dashTime > dashDuration - dashDuration/5)
        {
            anime.SetFloat("DashAngle", myAngle);
        }
        if(dashTime == 0)
        {
            anime.SetFloat("DashAngle", myAngle);
        }
        if (!lockOn)
        {
            anime.SetFloat("DashAngle", 0);
        }

        if (toggleSword)
        {
            anime.SetFloat("ItemState", 1);
            anime.SetFloat("DashState", 1);
        }else
        {
            anime.SetFloat("ItemState", 0);
            anime.SetFloat("DashState", 0);
        }
        
        if (movementPlayer.x != 0 || movementPlayer.z != 0)
        {
            activeState = StateOfTheAnimation.running;
        }
        if (movementPlayer.x == 0 && movementPlayer.z == 0 && !dash)
        {
            activeState = StateOfTheAnimation.idle;
        }
        if (activeState == StateOfTheAnimation.dashing)
        {
            anime.SetBool("Dashing", true);
            anime.SetBool("Running", false);
        }
        else
        {
            anime.SetBool("Dashing", false);
        }
        if (activeState == StateOfTheAnimation.running)
        {
            anime.SetBool("Running", true);
        }
        else
        {
            anime.SetBool("Running", false);
        }
    }



}
