using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlock : MonoBehaviour {

    public int pushersRequired;
    public List<string> pushers;

    private float pushingSpeed;
    private Vector3 movement;

	// Use this for initialization
	void Start () {
        GameObject playerManagerGO = GameObject.Find("PlayerManager");
        PlayerManager playerManager = playerManagerGO.GetComponent<PlayerManager>();
        pushingSpeed = playerManager.pushingSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (pushers.Count >= pushersRequired)
        {
            transform.Translate(movement * pushingSpeed * Time.deltaTime, Space.World);
        }
        movement = Vector3.zero;
    }

    public void AddPusher(GameObject pusher)
    {
        string pusherPrefix = pusher.GetComponent<PlayerControl>().playerPrefix;
        pushers.Add(pusherPrefix);
    }

    public void RemovePusher(GameObject pusher)
    {
        pushers.Remove(pusher.GetComponent<PlayerControl>().playerPrefix);
    }

    public bool Move(Vector3 movement, float speed)
    {
        this.movement = movement;
        pushingSpeed = speed;
        if (pushers.Count >= pushersRequired)
        {
            return true;
        }

        return false;
    }
}
