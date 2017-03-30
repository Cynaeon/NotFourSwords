using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlock : MonoBehaviour {

    public int pushersRequired;
    public List<int> pushers;
    public GameObject colliderRight;
    public GameObject colliderLeft;
    public GameObject colliderForward;
    public GameObject colliderBack;

    private float pushingSpeed;
    private Vector3 movement;

	void Start () {
        GameObject playerManagerGO = GameObject.Find("PlayerManager");
        PlayerManager playerManager = playerManagerGO.GetComponent<PlayerManager>();
        pushingSpeed = playerManager.pushingSpeed;
    }
	
	void Update () {
        if (pushers.Count >= pushersRequired)
        {
            bool moving = CheckColliders();
            if (moving)
            {
                transform.Translate(movement * pushingSpeed * Time.deltaTime, Space.World);
            }
        }
        movement = Vector3.zero;
    }

    private bool CheckColliders()
    {
        if (movement.normalized == Vector3.right && !colliderRight.GetComponent<PushBlockSideCollider>().collided)
        {
            return true;
        }
        else if (movement.normalized == Vector3.left && !colliderLeft.GetComponent<PushBlockSideCollider>().collided)
        {
            return true;
        }
        else if (movement.normalized == Vector3.forward && !colliderForward.GetComponent<PushBlockSideCollider>().collided)
        {
            return true;
        }
        else if (movement.normalized == Vector3.back && !colliderBack.GetComponent<PushBlockSideCollider>().collided)
        {
            return true;
        }
        return false;
    }

    public void AddPusher(GameObject pusher)
    {
        int pusherPrefix = (int)pusher.GetComponent<PlayerControl>().playerPrefix;
        pushers.Add(pusherPrefix);
    }

    public void RemovePusher(GameObject pusher)
    {
        pushers.Remove((int)pusher.GetComponent<PlayerControl>().playerPrefix);
    }

    public bool Move(Vector3 movement, float speed)
    {
        this.movement = movement;
        movement.y = 0;
        pushingSpeed = speed;
        if (pushers.Count >= pushersRequired)
        {
            return true;
        }

        return false;
    }
}
