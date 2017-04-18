using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Elevator : MonoBehaviour {

    public PlayerControl[] playerControls;
    public Canvas[] canvases; 
    public InputField[] inputFields;
    public Transform[] elevatorPositions;
    private Button[][] _buttons;
    private StandaloneInputModule _module;
    public string password;
    public Transform elevator;
    private bool _inMenu;
    private bool[] _isColliding;
    private bool[] _unlockedButtons;
    private bool _isInMotion;
    private GameManager gameManager;
    private int _destination;
    private float _maxDistanceDelta;
    public float speed;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerControls = new PlayerControl[5];
        for (int i = 1; i < 5; i++)
        {
            playerControls[i] = gameManager.players[i - 1].GetComponent<PlayerControl>();
        }
        _isColliding = new bool[5];
        _buttons = new Button[5][];
        _buttons[1] = new Button[9];
        _buttons[2] = new Button[9];
        _buttons[3] = new Button[9];
        _buttons[4] = new Button[9];

        GameObject[] canvases = GameObject.FindGameObjectsWithTag("Canvas");
        for (int i = 1; i <= 1; i++)
        {
            for (int j = 0; j <= 8; j++)
            {
                _buttons[i][j] = canvases[i].transform.GetChild(j).GetComponent<Button>();
            }
        }
        _module = EventSystem.current.GetComponent<StandaloneInputModule>();
    }

    void Update()
    {
        for (int i = 1; i <= 1; i++) {
            if (_isColliding[i])
            {
                if (!_inMenu && !_isInMotion)
                {
                    if (Input.GetButtonDown("P" + i + "_Action") && !_inMenu)
                    {
                        _inMenu = true;

                        playerControls[i].disableMovement = true;

                        _module.horizontalAxis = "P" + i + "_DPadHorizontal";
                        _module.verticalAxis = "P" + i + "_DPadVertical";
                        _module.submitButton = "P" + i + "_Action";
                        _module.cancelButton = "P" + i + "_Dash";

                        for (int j = 0; j <= 8; j++)
                        {
                            _buttons[i][j].gameObject.SetActive(true);

                            if (gameManager.floorsUnlocked[j])
                            {
                                if (j > 0)
                                {
                                    int tempJ = j;
                                    int tempI = i;
                                    _buttons[tempI][tempJ].onClick.AddListener(() =>
                                    {
                                        _destination = tempJ;
                                        playerControls[tempI].disableMovement = false;
                                        _isInMotion = true;

                                        _inMenu = false;

                                        for (int k = 0; k <= 8; k++)
                                        {
                                            _buttons[tempI][k].gameObject.SetActive(false);
                                        }

                                        EventSystem.current.SetSelectedGameObject(null);
                                    });
                                }
                            }
                        }
                        EventSystem.current.SetSelectedGameObject(_buttons[i][0].gameObject);
                    }
                }
                else
                {
                    if (Input.GetButtonDown("P" + i + "_Dash"))
                    {
                        _inMenu = false;

                        playerControls[i].disableMovement = false;

                        for (int j = 0; j <= 8; j++)
                        {
                            _buttons[i][j].gameObject.SetActive(false);
                        }

                        EventSystem.current.SetSelectedGameObject(null);
                    }
                }
            }
        }

        if (_isInMotion)
        {
            elevator.position = Vector3.MoveTowards(elevator.position, elevatorPositions[_destination].position, _maxDistanceDelta);
            //elevator.position = Vector3.Lerp(elevator.position, elevatorPositions[_destination].position, _t);
            _maxDistanceDelta += speed;
            Debug.Log(_maxDistanceDelta);

            if (Vector3.Distance(elevator.position, elevatorPositions[_destination].position) < 0.1f) 
            {
                _isInMotion = false;
                _maxDistanceDelta = 0;
            }
        }

        //EventSystem.current.SetSelectedGameObject(p1Input.gameObject);
        //p1Input.ActivateInputField();
    }

    

    void OnTriggerEnter(Collider other)
    {
        for (int i = 1; i <= 4; i++)
        {
            if (other == playerControls[i].playerHitbox.GetComponent<Collider>())
            {
                //canvases[i].GetComponent<UIManager>().EnableNotification(true);
                _isColliding[i] = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        for (int i = 1; i <= 4; i++)
        {
            if (other == playerControls[i].playerHitbox.GetComponent<Collider>())
            {
                //canvases[i].GetComponent<UIManager>().EnableNotification(false);
                _isColliding[i] = false;
                
            }
        }
    }
}
