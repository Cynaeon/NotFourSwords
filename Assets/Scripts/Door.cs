using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public bool open;

    private void Awake()
    {
        if (open)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        Destroy(gameObject);
    }

}
