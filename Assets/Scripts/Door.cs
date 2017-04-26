using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public bool open;
    public int doorID;

    private GameObject gameManager;
    private Vector3 endPos;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        open = gameManager.GetComponent<GameManager>().doorOpened[doorID];
        endPos = new Vector3(transform.position.x, transform.position.y - 10, transform.position.z);

        if (open)
        {
            OpenDoor();
            gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
        if (open)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * 3);
            if (Vector3.Distance(transform.position, endPos) <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
    

    public void OpenDoor()
    {
        gameManager.GetComponent<GameManager>().doorOpened[doorID] = true;
        open = true;
    }
}
