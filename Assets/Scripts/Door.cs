using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public bool open;

    private GameObject gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        open = gameManager.GetComponent<GameManager>().doorOpened_1;

        if (open)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        gameManager.GetComponent<GameManager>().doorOpened_1 = true;
        gameObject.SetActive(false);
    }

}
