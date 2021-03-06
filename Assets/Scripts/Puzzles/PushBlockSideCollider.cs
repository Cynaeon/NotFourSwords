﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlockSideCollider : MonoBehaviour {

    public bool collided;
    public bool playerCollided;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Walls" || other.tag == "Torch" || other.tag == "Player")
        {
            collided = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Walls" || other.tag == "Torch" || other.tag == "Player")
        {
            collided = false;
        }
    }
}
