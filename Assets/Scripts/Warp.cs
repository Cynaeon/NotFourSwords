using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour {

    public Transform destination;

    private bool shrinking;
    private GameObject player;
    private PlayerControl playerControl;

    private void Update()
    {
        if (player)
        {
            if (shrinking)
            {
                playerControl.disableMovement = true;
                player.transform.localScale -= new Vector3(0.1f, 0, 0.1f) * Time.deltaTime * 50;
                player.transform.position += new Vector3(0, 0.1f, 0);
                if (player.transform.localScale.x <= 0.1f)
                {
                    Vector3 exitPoint = destination.position;
                    player.transform.position = exitPoint;
                    player.transform.rotation = destination.rotation;
                    shrinking = false;
                    playerControl.disableMovement = false;
                }
            }
            else
            {
                player.transform.localScale += new Vector3(0.1f, 0, 0.1f) * Time.deltaTime * 50;
                if (player.transform.localScale.x >= 1)
                {
                    player.transform.localScale = Vector3.one;
                    player = null;
                }
            }
        }
    }



	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.transform.localScale.x == 1)
            {
                player = other.gameObject;
                playerControl = other.GetComponent<PlayerControl>();
                shrinking = true;
            }
            /*
            Vector3 exitPoint = destination.position + destination.forward * 3;
            other.transform.position = exitPoint;
            other.transform.rotation = destination.rotation;
            */
        }
    }
}
