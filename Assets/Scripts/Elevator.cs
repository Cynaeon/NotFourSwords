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
    public GameManager gameManager;
    private int _destination;
    private float _t;
    public float speed;

    void Awake()
    {
        _isColliding = new bool[5];
        _buttons = new Button[5][];
        _buttons[1] = new Button[9];
        _buttons[2] = new Button[9];
        _buttons[3] = new Button[9];
        _buttons[4] = new Button[9];
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
        for (int i = 1; i <= 4; i++) {
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

                            //if (gameManager.floorsUnlocked[j])
                            //{
                            if (j > 0)
                            {
                                int temp = j;
                                _buttons[i][temp].onClick.AddListener(() =>
                                {
                                    _destination = temp;

                                    _isInMotion = true;
                                });
                            }
                            //}
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
            elevator.position = Vector3.Lerp(elevator.position, elevatorPositions[_destination].position, _t);
            _t += speed;
            Debug.Log(_t);
            if (_t >= 1)
            {
                _isInMotion = false;
                _t = 0;
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
