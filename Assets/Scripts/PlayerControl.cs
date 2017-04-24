using System;
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
        sword,
        key
    }
    private Items myItem;

    public enum StateOfTheAnimation
    {
        idle,
        idle_Sword,
        running,
        running_Sword,
        dashing,
        jumping,
        falling,
        landing
    }
    private StateOfTheAnimation activeState;
    private Animator anime;

    #region Public Objects
    public Transform playerCamera;
    public Camera _playerCamera;
    public Canvas _playerCanvas;
    public GameObject bolt;
    public GameObject playerHitbox;
    public GameObject trailModel;
    public GameObject playerModel;
    public GameObject crossbow;
    public GameObject SwordItem;
    public GameObject Sheath;
    public GameObject SheathedSword;
    public TrailRenderer swordTrail;
    public GameObject MonocleItem;
    public GameObject MagnetItem;
    public GameObject MagnetEffect;
    public GameObject LeftBoot;
    public GameObject RightBoot;
    public GameObject SwordHitBox;
    #endregion

    #region Player Attributes
    [HideInInspector]
    public int maxHealth;
    [HideInInspector]
    public int currentHealth;
    [HideInInspector]
    public int gatheredScore;
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
    private float deadzone;
    #endregion

    #region Other Objects
    private CharacterController controller;
    private Transform lockOnArrow;
    private Renderer lockOnRend;
    private Color lockOnGreen;
    private Color lockOnRed;
    private GameObject pushBlock;
    private Renderer _rend;
    #endregion

    #region Private Variables
    private GameManager gameManager;
    private PlayerManager playerManager;
    private Vector3 movementPlayer;
    private TrailRenderer trailRend;
    private float currentSpeed;
    private float verticalVelocity;
    private float gravity;
    private float jumpForce;
    private Vector3 playerPos;
    private float dashTime;
    private float fallTime;
    private float afterImageTime;
    private float lastShot;
    private float currentInvulTime;
    private int burstCount;
    //private float burstSpeed;
    private Transform lockOnTarget = null;
    private Vector3 dashDir;
    private Vector3 slidingDir;
    private float _switchTargetAxis;
    private int switchTarget;
    private Color defaultColor;
    private GameObject lastBounced;
    private float swordSwing;
    #endregion

    #region Private Booleans
    private bool burstShot;
    private bool lockOn;
    [HideInInspector]
    public bool firstPerson;
    private bool grabbing;
    private bool climbing;
    private bool Jumped;
    private bool canSee;
    private bool canChangeItem;
    private bool canOpenDoor;
    private bool canDash;
    [HideInInspector]
    public bool disableMovement;
    private bool dash;
    private bool toggleSword;
    private bool invulnerable;
    private bool _magnetActive;
    private bool sliding;
    [HideInInspector]
    public bool settingStartPos;
    private FogDensity fogDensity;
    private bool _yAxisPressed;
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
        gatheredScore = playerManager.score;
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
        gravity = playerManager.gravity;
        jumpForce = playerManager.jumpForce;
        lockOnArrow = transform.Find("LockOnArrow");
        lockOnGreen = lockOnArrow.GetComponent<Renderer>().material.color;
        lockOnRed = Color.red;
        playerPos = transform.position;
        myItem = Items.none;
        _maxMagnetDistance = playerManager.maxMagnetDistance;
        _magnetVelocity = playerManager.magnetVelocity;
        _minMagnetDistance = playerManager.minMagnetDistance;
        _grabSpot = GetComponentInChildren<BoxCollider>();
        deadzone = playerManager.deadzone;
        #endregion
        trailRend = GetComponent<TrailRenderer>();
        trailRend.time = 0;
        swordTrail = SwordItem.GetComponent<TrailRenderer>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetActivity(false, false, false, false, false, false, true);
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

            if (playerPrefix != Players.NotSelected && !gameManager.disableMovement && !disableMovement)
            {
                GetMovement();
                Health();
                Dashing();
                Magnet();
                FirstPersonControls();
                Grabbing();
                Movement();
                Sliding();
                LockOnSystem();
                Shooting();
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

        movementPlayer = new Vector2(Input.GetAxis(playerPrefix + "Horizontal"), Input.GetAxis(playerPrefix + "Vertical"));
        if (movementPlayer.magnitude < deadzone)
        {
            movementPlayer = Vector2.zero;
        }
        else
        {
            movementPlayer = (moveHorizontal * right + moveVertical * forward);
        }
    }


    private void Sliding()
    {
        if (sliding)
        {
            trailRend.time = 0.5f;
            controller.Move(slidingDir * currentSpeed * 2 * Time.deltaTime);
        }
        else
        {
            trailRend.time = 0;
        }
    }

    private void Movement()
    {
        if (!sliding)
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
                // Easing goes here I guess
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
                if (_magnetActive) {
                    movementPlayer = new Vector3(0, 0, 0);
                }

                movementPlayer.y = verticalVelocity;
                controller.Move(movementPlayer * currentSpeed * Time.deltaTime);
            }
        }
    }

    private void Grabbing()
    {
        if (Input.GetButton(playerPrefix + "Item"))
        {
            _grabSpot.enabled = false;
        }
        else
        {
            _grabSpot.enabled = true;
        }

        if (grabbing && !Input.GetButton(playerPrefix + "Action"))
        {
            pushBlock.GetComponent<PushBlock>().RemovePusher(gameObject);
            pushBlock = null;
            grabbing = false;
            currentSpeed = defaultSpeed;
            if (!toggleSword)
            {
                crossbow.SetActive(true);
            }
        }

        if (grabbing && !firstPerson)
        {
            crossbow.SetActive(false);
            Vector3 direction = transform.position - pushBlock.transform.position;
            direction = direction.normalized;
            SnapPlayerRotation(direction);

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

    private void SnapPlayerRotation(Vector3 direction)
    {
        direction = transform.eulerAngles;
        direction.y = Mathf.Round(direction.y / 90) * 90;
        transform.eulerAngles = direction;
    }

    private void Gravity()
    {
        if (controller.isGrounded)
        {
            canDash = true;
            verticalVelocity = -gravity * Time.deltaTime;
            if (playerPrefix != Players.NotSelected)
            {
                if (Input.GetButton(playerPrefix + "Item") && myItem == Items.jump)
                {
                    verticalVelocity = jumpForce;
                    Jumped = true;
                }
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
            int itemAvailable = FindClosestGameObjectWithTag("ItemSpawner").gameObject.GetComponent<ItemSpawner>().checkActive();
            if (itemAvailable > 0 && itemAvailable != (int)myItem || itemAvailable == 0 && myItem != Items.none)
            {
                _playerCanvas.GetComponent<UIManager>().EnableNotification(true);
            }
            if (Input.GetButtonDown(playerPrefix + "Action"))
            {
                FindClosestGameObjectWithTag("ItemSpawner").gameObject.GetComponent<ItemSpawner>().changeActive((int)myItem);
                switch (itemAvailable)
                {
                    // Take the & symbols out from the cullingMask assignments below if lens (or something) doesn't work
                    case 0:
                        myItem = Items.none;
                        _playerCanvas.GetComponent<UIManager>().UIItems(false, false, false, false, false);
                        SetActivity(false, false, false, false, false, false, true);
                        fogDensity.fadeState(false);
                        toggleSword = false;
                        _playerCamera.cullingMask &= ~(1 << 8);
                        canSee = false;
                        break;
                    case 1:
                        myItem = Items.jump;
                        _playerCanvas.GetComponent<UIManager>().UIItems(true, false, false, false, false);
                        fogDensity.fadeState(false);
                        SetActivity(false, false, false, false, false, true, true);
                        toggleSword = false;
                        _playerCamera.cullingMask &= ~(1 << 8);
                        canSee = false;
                        break;
                    case 2:
                        myItem = Items.seeThrough;
                        _playerCanvas.GetComponent<UIManager>().UIItems(false, true, false, false, false);
                        SetActivity(false, false, false, true, false, false, true);
                        toggleSword = false;
                        canSee = true;
                        break;
                    case 3:
                        myItem = Items.magnet;
                        _playerCanvas.GetComponent<UIManager>().UIItems(false, false, true, false, false);
                        SetActivity(false, false, false, false, true, false, true);
                        fogDensity.fadeState(false);
                        _playerCamera.cullingMask &= ~(1 << 8);
                        toggleSword = false;
                        canSee = false;
                        break;
                    case 4:
                        myItem = Items.sword;
                        _playerCanvas.GetComponent<UIManager>().UIItems(false, false, false, true, false);
                        SetActivity(false, true, true, false, false, false, true);
                        fogDensity.fadeState(false);
                        _playerCamera.cullingMask &= ~(1 << 8);
                        canSee = false;
                        break;
                    case 5:
                        myItem = Items.key;
                        _playerCanvas.GetComponent<UIManager>().UIItems(false, false, false, false, true);
                        SetActivity(false, false, false, false, false, false, true);
                        fogDensity.fadeState(false);
                        _playerCamera.cullingMask &= ~(1 << 8);
                        canSee = false;
                        break;
                }
            }
        }

        if (canOpenDoor)
        {
            if (myItem == Items.key)
            {
                if (Input.GetButtonDown(playerPrefix + "Item"))
                {
                    FindClosestGameObjectWithTag("Door").gameObject.GetComponent<Door>().OpenDoor();
                    myItem = Items.none;
                    _playerCanvas.GetComponent<UIManager>().UIItems(false, false, false, false, false);
                }
            }
        }
        else
        {
        }

    }

    private void SetActivity(bool _sword, bool _sheath, bool _sheathedSword, bool _monocle, bool _magnet, bool _boots, bool _crossbow)
    {
        SwordItem.SetActive(_sword);
        Sheath.SetActive(_sheath);
        SheathedSword.SetActive(_sheathedSword);
        MonocleItem.SetActive(_monocle);
        MagnetItem.SetActive(_magnet);
        LeftBoot.SetActive(_boots);
        RightBoot.SetActive(_boots);
        crossbow.SetActive(_crossbow);
    }

    private void Lens()
    {
        if (Input.GetButtonDown(playerPrefix + "Item") && myItem == Items.seeThrough)
        {
            if (canSee)
            {
                // Lens on
                fogDensity.fadeState(true);
                _playerCamera.clearFlags = CameraClearFlags.SolidColor;
                _playerCamera.cullingMask |= (1 << 8);
                _playerCamera.cullingMask = ~(1 << 10);
                canSee = false;
            }
            else
            {
                // Lens off
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
                crossbow.SetActive(false);
                SheathedSword.SetActive(false);
                toggleSword = true;
            }
            else
            {
                toggleSword = false;
                SwordItem.SetActive(false);
                crossbow.SetActive(true);
                SheathedSword.SetActive(true);
            }
        }
    }

    private void Health()
    {
        // When invulnerable after taking dmg, flash red and disable hitbox
        if (invulnerable && currentInvulTime <= dmgInvulTime)
        {
            playerHitbox.SetActive(false);
            _rend.material.color = Color.Lerp(defaultColor, Color.red, Mathf.PingPong(Time.time * 5, 1));
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
            currentHealth = maxHealth;
            _playerCanvas.GetComponent<UIManager>().UpdateHealth(currentHealth);
        }
    }

    private void Magnet()
    {
        // Magnet
        // Works when no GrabSpot is present
        if (Input.GetButton(playerPrefix + "Item") && myItem == Items.magnet && !climbing && !grabbing)
        {
            _magnetActive = true;
            MagnetEffect.SetActive(true);
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
                        Vector3 dir = transform.position - hit.transform.position;
                        dir = dir.normalized;
                        Debug.DrawRay(hit.transform.position, dir, Color.green, 2);
                        hit.transform.Translate(dir * Time.deltaTime * _magnetVelocity, Space.Self);
                    }
                }
            }
        }
        else
        {
            _magnetActive = false;
            MagnetEffect.SetActive(false);
        }
    }

    private void Dashing()
    {
        if (dashTime == 0 && Input.GetButtonDown(playerPrefix + "Dash") && !sliding)
        {
            if (movementPlayer == Vector3.zero)
            {
                movementPlayer = transform.forward.normalized;
            }
            if (canDash && !climbing && !_magnetActive)
            {
                if (lockOn)
                {
                    float myAngle = Mathf.Atan2(Input.GetAxis(playerPrefix + "Horizontal"), Input.GetAxis(playerPrefix + "Vertical")) * Mathf.Rad2Deg;
                    anime.SetFloat("DashAngle", myAngle);
                }
                else
                {
                    anime.SetFloat("DashAngle", 0);
                }
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

    private GameObject FindClosestGameObjectWithTag(string tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tag);
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

    public void TakeDamage(int dmg, Vector3 dir)
    {
        invulnerable = true;
        gameObject.GetComponent<ImpactReceiver>().AddImpact(-dir + Vector3.up * 2, 100);
        movementPlayer = -movementPlayer;
        currentHealth -= dmg;
        _playerCanvas.GetComponent<UIManager>().UpdateHealth(currentHealth);
    }

    public void HealDamage(int heal)
    {
        if (currentHealth != maxHealth)
        {
            currentHealth += heal;
        }
        _playerCanvas.GetComponent<UIManager>().UpdateHealth(currentHealth);
    }

    public void IncreaseScore(int amount)
    {
        gatheredScore = gatheredScore + amount;
    }

    public void IncreaseShootingLevel(int upgrade)
    {
        shootingLevel += upgrade;
    }

    private void Shooting()
    {
        if (!dash && !toggleSword && !climbing && !_magnetActive && !grabbing)
        {

            if (shootingLevel == 0)
            {
                shootingSpeed = 0.5f;
                if (Input.GetButtonDown(playerPrefix + "Shoot") && lastShot > shootingSpeed)
                {
                    Shoot();
                }
            }
            else if (shootingLevel == 1)
            {
                shootingSpeed = 0.3f;
                if (Input.GetButtonDown(playerPrefix + "Shoot") && lastShot > shootingSpeed)
                {
                    Shoot();
                }
            }
            else if (shootingLevel >= 2)
            {
                shootingSpeed = 0.1f;
                if (Input.GetButton(playerPrefix + "Shoot") && lastShot > shootingSpeed)
                {
                    Shoot();
                }
            }
            lastShot += Time.deltaTime;
        }
        if (toggleSword)
        {
            if (Input.GetButtonDown(playerPrefix + "Shoot"))
            {
                swordSwing = 0.8f;
                anime.SetTrigger("Sword");
                SwordHitBox.SetActive(true);
            }

            if (swordSwing > 0)
            {
                swordTrail.enabled = true;
                swordSwing -= Time.deltaTime;
            }
            else
            {
                swordTrail.enabled = false;
            }
        }
    }

    private void Shoot()
    {
        //Vector3 pos = new Vector3(transform.position.x, transform.position.y + .6f, transform.position.z);
        //Instantiate(bolt, pos + transform.forward, transform.rotation);
        Vector3 pos = new Vector3(crossbow.transform.position.x, crossbow.transform.position.y, crossbow.transform.position.z);
        Quaternion rot = crossbow.transform.rotation;
        rot *= Quaternion.Euler(90, 0, 0);
        Instantiate(bolt, pos, rot);
        lastShot = 0;
    }

    private void FirstPersonControls()
    {
        if (Input.GetAxis(playerPrefix + "FirstPerson") > 0.5 && movementPlayer == Vector3.zero && !lockOn)
        {
            if (!firstPerson)
            {
                //Vector3 rot = new Vector3(playerCamera.transform.rotation.x, 0, playerCamera.transform.rotation.z);
                Vector3 dir = transform.position - playerCamera.transform.position;
                dir.y = 0;
                transform.rotation = Quaternion.LookRotation(dir);

            }
            firstPerson = true;
        }
        else
        {
            firstPerson = false;
        }
        if (firstPerson)
        {
            if (playerPrefix == Players.P1_)
            {
                _playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player1"));
            }
            else if (playerPrefix == Players.P2_)
            {
                _playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player2"));
            }
            else if (playerPrefix == Players.P3_)
            {
                _playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player3"));
            }
            else if (playerPrefix == Players.P4_)
            {
                _playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player4"));
            }
            float lookHorizontal = Input.GetAxis(playerPrefix + "HorizontalRightStick");
            float lookVertical = Input.GetAxis(playerPrefix + "VerticalRightStick");
            Vector3 lookPlayer = new Vector3(0, -lookHorizontal, 0);
            transform.localEulerAngles += lookPlayer;
        }
        else
        {
            if (playerPrefix == Players.P1_)
            {
                _playerCamera.cullingMask |= (1 << LayerMask.NameToLayer("Player1"));
            }
            else if (playerPrefix == Players.P2_)
            {
                _playerCamera.cullingMask |= (1 << LayerMask.NameToLayer("Player2"));
            }
            else if (playerPrefix == Players.P3_)
            {
                _playerCamera.cullingMask |= (1 << LayerMask.NameToLayer("Player3"));
            }
            else if (playerPrefix == Players.P4_)
            {
                _playerCamera.cullingMask |= (1 << LayerMask.NameToLayer("Player4"));
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

        //Debug.Log("Target: " + switchTarget + "\nArray size: " + enemyList.Count);
        //foreach (GameObject go in enemyList)
        //{
        //    Debug.Log(go.name);
        //}

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


            // WIP Vertical Target Switching.
            // Is literally bending logic and might actually cause the universe to collapse.
            // It's a quantum nightmare. Be careful not to read it, your existence might end without warning.
            // On a more serious note,
            // H E L P

            //if (Input.GetButtonDown(playerPrefix + "SwitchTarget"))
            //Debug.Log(Input.GetAxis(playerPrefix + "VerticalRightStick"));
            if (lockOnTarget != null)
            {
                if (Input.GetAxis(playerPrefix + "VerticalRightStick") > 0.25)
                {

                    if (switchTarget < enemyList.Count - 1 && !_yAxisPressed)
                    {
                        switchTarget++;
                        _yAxisPressed = true;
                    }
                    lockOnTarget = enemyList[switchTarget].transform;
                }
                else if (Input.GetAxis(playerPrefix + "VerticalRightStick") < -0.25)
                {
                    if (switchTarget > 0 && !_yAxisPressed)
                    {
                        switchTarget--;
                        _yAxisPressed = true;
                    }
                    lockOnTarget = enemyList[switchTarget].transform;
                }
                else
                {
                    _yAxisPressed = false;
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "PushBlock")
        {
            if (Input.GetButtonDown(playerPrefix + "Action"))
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
                    /*
                    Vector3 pos = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
                    pos += Vector3.forward;
                    transform.position = pos;
                    */
                    //transform.rotation = other.transform.forward;
                    Vector3 direction = transform.position - other.transform.position;
                    direction = direction.normalized;
                    SnapPlayerRotation(direction);
                    climbing = true;
                }
                else
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
            if (!sliding)
            {
                if (movementPlayer.x != 0 && movementPlayer.z != 0)
                {
                    slidingDir = movementPlayer.normalized;
                    sliding = true;
                }
            }
        }
    }

    public void ItemStateChange(bool changeTo)
    {
        if (!changeTo)
        {
            _playerCanvas.GetComponent<UIManager>().EnableNotification(false);
        }
        canChangeItem = changeTo;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Door")
        {
            canOpenDoor = true;
        }

        if (other.tag == "SlidingIce")
        {
            Debug.Log(movementPlayer);
            if (movementPlayer.x != 0 && movementPlayer.z != 0)
            {
                slidingDir = movementPlayer.normalized;
                sliding = true;
            }
            else if (dash)
            {
                slidingDir = dashDir.normalized;
                sliding = true;
            }
        }

        if (other.tag == "SlideStopper")
        {
            if (sliding)
            {
                slidingDir = Vector3.zero;
                sliding = false;
            }
        }

        if (other.tag == "SlideBouncer")
        {
            if (sliding)
            {
                if (lastBounced != other.gameObject)
                {
                    slidingDir = Vector3.Reflect(slidingDir, other.transform.forward);
                    // This might be a bad idea...
                    slidingDir *= 1.1f;
                    lastBounced = other.gameObject;
                }
            }
        }
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

        if (other.tag == "Door")
        {
            canOpenDoor = false;
        }

        if (other.tag == "SlidingIce")
        {
            sliding = false;
        }
    }

    public void Animations()
    {

        if (toggleSword)
        {
            anime.SetFloat("SwordState", 1);
            anime.SetFloat("DashState", 1);
        }
        else
        {
            anime.SetFloat("SwordState", 0);
            anime.SetFloat("DashState", 0);
        }

        if (controller.velocity.y < -8)
        {
            Jumped = false;
            anime.SetBool("Falling", true);
            anime.SetBool("Running", false);
            anime.SetBool("Jumping", false);
            activeState = StateOfTheAnimation.falling;
        }

        if (controller.velocity.y > -2 && Jumped)
        {
            anime.SetBool("Jumping", true);
            activeState = StateOfTheAnimation.jumping;
        }

        if (controller.velocity.z > 0.1 || controller.velocity.x > 0.1 || controller.velocity.z < -0.1 || controller.velocity.x < -0.1)
        {
            if (!dash)
            {
                activeState = StateOfTheAnimation.running;
                anime.SetBool("Running", true);
            }
        }
        else
        {
            anime.SetBool("Running", false);
        }

        if (controller.velocity == Vector3.zero && activeState != StateOfTheAnimation.falling)
        {
            activeState = StateOfTheAnimation.idle;
            anime.SetBool("Running", false);
        }
        if (dash)
        {
            activeState = StateOfTheAnimation.dashing;
            anime.SetBool("Running", false);
            anime.SetBool("Dashing", true);
        }
        else
        {
            anime.SetBool("Dashing", false);
        }

        if (controller.velocity == Vector3.zero)
        {
            anime.SetBool("Idle", true);
        }
        else
        {
            anime.SetBool("Idle", false);
        }

        if (controller.isGrounded)
        {
            anime.SetBool("IsGrounded", true);
            anime.SetBool("Landing", true);
            anime.SetBool("Falling", false);
            anime.SetBool("Jumping", false);
        }
        else

        {

            anime.SetBool("IsGrounded", false);
            anime.SetBool("Landing", false);
        }

        if (grabbing)
        {
            if (toggleSword)
            {
                SwordItem.SetActive(false);
                SheathedSword.SetActive(true);
            }

            if (Vector3.Dot(transform.forward, movementPlayer) > 0)
            {
                anime.SetFloat("PushState", 1);
            }
            if (Vector3.Dot(transform.forward, movementPlayer) < 0)
            {
                anime.SetFloat("PushState", 0);
            }
            if (Vector3.Dot(transform.forward, movementPlayer) == 0)
            {
                anime.enabled = false;
            }
            else
            {
                anime.enabled = true;
            }
            anime.SetBool("Pushing", true);
        }
        else
        {
            anime.enabled = true;
            if (toggleSword)
            {
                SwordItem.SetActive(true);
                SheathedSword.SetActive(false);
            }
            anime.SetBool("Pushing", false);
        }
        if (firstPerson)
        {
            anime.SetBool("FirstPerson", true);
            float lookVertical = Input.GetAxis(playerPrefix + "VerticalRightStick");
            Vector3 lookPlayer = new Vector3(-lookVertical, 0, 0);
            anime.SetFloat("AimState", anime.GetFloat("AimState") + lookVertical);

            if (anime.GetFloat("AimState") < -90)
            {
                anime.SetFloat("AimState", -90);
            }

            if (anime.GetFloat("AimState") > 90)
            {
                anime.SetFloat("AimState", 90);
            }

        }
        else
        {
            anime.SetFloat("AimState", 0);
            anime.SetBool("FirstPerson", false);
        }

        if (Input.GetButton(playerPrefix + "Shoot") && !toggleSword && !grabbing && !Jumped && !firstPerson && !dash)
        {
            anime.SetBool("Shooting", true);
        }
        else
        {
            anime.SetBool("Shooting", false);
        }
        if (climbing && Vector3.Dot(transform.up, movementPlayer) > 0)
        {
            anime.SetBool("Climbing", true);
            anime.SetFloat("ClimbState", 1);
            anime.enabled = true;
        }
        if (climbing && Vector3.Dot(transform.up, movementPlayer) < 0)
        {
            anime.SetBool("Climbing", true);
            anime.SetFloat("ClimbState", -1);
            anime.enabled = true;
        }
        if (climbing && Vector3.Dot(transform.up, movementPlayer) == 0)
        {
            anime.SetBool("Climbing", true);
            anime.enabled = false;
        }
        if (!climbing)
        {
            anime.SetBool("Climbing", false);
        }
        if (_magnetActive)
        {
            anime.SetBool("Magnet", true);
        }else
        {
            anime.SetBool("Magnet", false);
        }
    }
}