using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public bool open;
    public int doorID;

    private GameObject gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        open = gameManager.GetComponent<GameManager>().doorOpened[doorID];

        if (open)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        gameManager.GetComponent<GameManager>().doorOpened[doorID] = true;
        gameObject.SetActive(false);
    }

}
