using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Elevator : MonoBehaviour {

    public PlayerControl[] playerControls;
    public Transform[] buttonContainers;
    public InputField[] inputFields;
    public Transform[] elevatorPositions;
    private Button[][] _buttons;
    private StandaloneInputModule _module;
    public Transform elevator;
    private bool _inMenu;
    private bool[] _isColliding;
    private bool[] _unlockedButtons;
    private bool _isInMotion;
    private GameManager gameManager;
    private int _destination;
    private float _maxDistanceDelta;
    public float speed;
    private bool _typing;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerControls = new PlayerControl[5];
        _buttons = new Button[5][];
        for (int i = 1; i < 5; i++)
        {
            playerControls[i] = gameManager.players[i - 1].GetComponent<PlayerControl>();
            _buttons[i] = new Button[9];
        }
        _isColliding = new bool[5];
        
        for (int i = 1; i <= 4; i++)
        {
            for (int j = 0; j <= 8; j++)
            {
                _buttons[i][j] = buttonContainers[i].transform.GetChild(j).GetComponent<Button>();
            }
        }
        _module = EventSystem.current.GetComponent<StandaloneInputModule>();
        elevator.position = elevatorPositions[gameManager.currentFloor].position;
    }

    void Update()
    {
        for (int i = 1; i <= 4; i++) {
            if (_isColliding[i])
            {
                if (!_inMenu && !_isInMotion)
                {
                    if (Input.GetButtonDown("P" + i + "_Action"))
                    {
                        _inMenu = true;

                        playerControls[i].disableMovement = true;

                        _module.horizontalAxis = "P" + i + "_DPadHorizontal";
                        _module.verticalAxis = "P" + i + "_DPadVertical";
                        _module.submitButton = "P" + i + "_Action";
                        _module.cancelButton = "P" + i + "_Dash";

                        for (int j = 0; j <= 8; j++)
                        {
                            _buttons[i][0].gameObject.SetActive(true);
                            
                            if (j > 0)
                            { 
                                if (gameManager.floorsUnlocked[j])
                                {
                                    _buttons[i][j].gameObject.SetActive(true);
                                    int tempJ = j;
                                    int tempI = i;
                                    _buttons[tempI][tempJ].onClick.AddListener(() =>
                                    {
                                        if (gameManager.currentFloor != tempJ)
                                        {
                                            _destination = tempJ;
                                            gameManager.currentFloor = tempJ;
                                            playerControls[tempI].disableMovement = false;
                                            _isInMotion = true;

                                            _inMenu = false;

                                            for (int k = 0; k <= 8; k++)
                                            {
                                                _buttons[tempI][k].gameObject.SetActive(false);
                                            }

                                            EventSystem.current.SetSelectedGameObject(null);
                                        }
                                    });
                                }
                            } else
                            {
                                int tempI = i;
                                _buttons[tempI][0].onClick.AddListener(() =>
                                {
                                    inputFields[tempI].gameObject.SetActive(true);
                                    EventSystem.current.SetSelectedGameObject(inputFields[tempI].gameObject);
                                    _typing = true;
                                });
                            }
                        }
                        EventSystem.current.SetSelectedGameObject(_buttons[i][0].gameObject);
                    }
                }
                else
                {
                    if (Input.GetButtonDown("P" + i + "_Dash"))
                    {
                        if (!_typing)
                        {
                            _inMenu = false;

                        playerControls[i].disableMovement = false;

                        for (int j = 0; j <= 8; j++)
                        {
                            _buttons[i][j].gameObject.SetActive(false);
                        }

                        
                            EventSystem.current.SetSelectedGameObject(null);
                        } else
                        {
                            inputFields[i].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        for (int i = 1; i <= 4; i++)
        {
            
            if (inputFields[i] != null)
            {

                int tempI = i;
                inputFields[i].onEndEdit.AddListener((string contents) =>
                {
                    switch (contents)
                    {
                        case "2":
                            gameManager.floorsUnlocked[2] = true;
                            _buttons[tempI][2].gameObject.SetActive(true);
                            _buttons[tempI][2].onClick.AddListener(() =>
                            {
                                if (gameManager.currentFloor != 2)
                                {
                                    _destination = 2;
                                    gameManager.currentFloor = 2;
                                    playerControls[tempI].disableMovement = false;
                                    _isInMotion = true;

                                    _inMenu = false;

                                    for (int k = 0; k <= 8; k++)
                                    {
                                        _buttons[tempI][k].gameObject.SetActive(false);
                                    }

                                    EventSystem.current.SetSelectedGameObject(null);
                                }
                            });
                            gameManager.attempts = 0;
                            break;
                        case "3":
                            gameManager.floorsUnlocked[3] = true;
                            _buttons[tempI][3].gameObject.SetActive(true);
                            _buttons[tempI][3].onClick.AddListener(() =>
                            {
                                if (gameManager.currentFloor != 3)
                                {
                                    _destination = 3;
                                    gameManager.currentFloor = 3;
                                    playerControls[tempI].disableMovement = false;
                                    _isInMotion = true;

                                    _inMenu = false;

                                    for (int k = 0; k <= 8; k++)
                                    {
                                        _buttons[tempI][k].gameObject.SetActive(false);
                                    }

                                    EventSystem.current.SetSelectedGameObject(null);
                                }
                            });
                            gameManager.attempts = 0;
                            break;
                        case "4":
                            gameManager.floorsUnlocked[4] = true;
                            
                            _buttons[tempI][4].gameObject.SetActive(true);
                            _buttons[tempI][4].onClick.AddListener(() =>
                            {
                                if (gameManager.currentFloor != 4)
                                {
                                    _destination = 4;
                                    gameManager.currentFloor = 4;
                                    playerControls[tempI].disableMovement = false;
                                    _isInMotion = true;

                                    _inMenu = false;

                                    for (int k = 0; k <= 8; k++)
                                    {
                                        _buttons[tempI][k].gameObject.SetActive(false);
                                    }

                                    EventSystem.current.SetSelectedGameObject(null);
                                }
                            });
                            gameManager.attempts = 0;
                            break;
                        case "5":
                            gameManager.floorsUnlocked[5] = true;
                            
                            _buttons[tempI][5].gameObject.SetActive(true);
                            _buttons[tempI][5].onClick.AddListener(() =>
                            {
                                if (gameManager.currentFloor != 5)
                                {
                                    _destination = 5;
                                    gameManager.currentFloor = 5;
                                    playerControls[tempI].disableMovement = false;
                                    _isInMotion = true;

                                    _inMenu = false;

                                    for (int k = 0; k <= 8; k++)
                                    {
                                        _buttons[tempI][k].gameObject.SetActive(false);
                                    }

                                    EventSystem.current.SetSelectedGameObject(null);
                                }
                            });
                            gameManager.attempts = 0;
                            break;
                        case "6":
                            gameManager.floorsUnlocked[6] = true;
                            
                            _buttons[tempI][6].gameObject.SetActive(true);
                            _buttons[tempI][6].onClick.AddListener(() =>
                            {
                                if (gameManager.currentFloor != 6)
                                {
                                    _destination = 6;
                                    gameManager.currentFloor = 6;
                                    playerControls[tempI].disableMovement = false;
                                    _isInMotion = true;

                                    _inMenu = false;

                                    for (int k = 0; k <= 8; k++)
                                    {
                                        _buttons[tempI][k].gameObject.SetActive(false);
                                    }

                                    EventSystem.current.SetSelectedGameObject(null);
                                }
                            });
                            gameManager.attempts = 0;
                            break;
                        case "7":
                            gameManager.floorsUnlocked[7] = true;
                            
                            _buttons[tempI][7].gameObject.SetActive(true);
                            _buttons[tempI][7].onClick.AddListener(() =>
                            {
                                if (gameManager.currentFloor != 7)
                                {
                                    _destination = 7;
                                    gameManager.currentFloor = 7;
                                    playerControls[tempI].disableMovement = false;
                                    _isInMotion = true;

                                    _inMenu = false;

                                    for (int k = 0; k <= 8; k++)
                                    {
                                        _buttons[tempI][k].gameObject.SetActive(false);
                                    }

                                    EventSystem.current.SetSelectedGameObject(null);
                                }
                            });
                            gameManager.attempts = 0;
                            break;
                        default:
                            gameManager.attempts++;
                            if (gameManager.attempts == 10)
                            {
                                // Snape kills Dumbledore
                            }
                            break;
                    }
                    inputFields[tempI].text = null;
                    inputFields[tempI].gameObject.SetActive(false);
                    inputFields[tempI].onEndEdit.RemoveAllListeners();
                    EventSystem.current.SetSelectedGameObject(_buttons[tempI][0].gameObject);
                    _typing = false;
                });
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
                gameManager.canvases[i].GetComponent<UIManager>().EnableNotification(true);
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
                gameManager.canvases[i].GetComponent<UIManager>().EnableNotification(false);
                _isColliding[i] = false;
                
            }
        }
    }
}
