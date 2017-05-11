using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCall : MonoBehaviour
{

    public Elevator elevator;
    private GameManager _gameManager;
    private PlayerControl[] _playerControls;
    public int floor;
    private bool[] _inRange;

    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _playerControls = new PlayerControl[5];
        for (int i = 1; i < 5; i++)
        {
            _playerControls[i] = _gameManager.players[i - 1].GetComponent<PlayerControl>();
        }
        _inRange = new bool[5];
    }

    void Update()
    {
        for (int i = 1; i <= 4; i++)
        {
            if (_inRange[i] && !elevator.isInMotion)
            {
                if (Input.GetButtonDown("P" + i + "_Action"))
                {
                    elevator.isInMotion = true;
                    _gameManager.currentFloor = floor;
                    _gameManager.floorsUnlocked[floor] = true;
                    elevator.destination = floor;
                }
            }
            else
            {
                _gameManager.canvases[i].GetComponent<UIManager>().EnableNotification(false);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(_gameManager.currentFloor);
        for (int i = 1; i <= 4; i++)
        {
            if (other == _playerControls[i].playerHitbox.GetComponent<Collider>() && !elevator.isInMotion && _gameManager.currentFloor != floor)
            {
                _gameManager.canvases[i].GetComponent<UIManager>().EnableNotification(true);
                _inRange[i] = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        for (int i = 1; i <= 4; i++)
        {
            if (other == _playerControls[i].playerHitbox.GetComponent<Collider>())
            {
                _gameManager.canvases[i].GetComponent<UIManager>().EnableNotification(false);
                _inRange[i] = false;
            }
        }
    }
}
