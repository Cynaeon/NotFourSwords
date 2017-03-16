using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBox : MonoBehaviour
{

    public GameObject player;

    private PlayerControl _playerControl;
    private int playerPrefix;

    void Start()
    {
        _playerControl = player.GetComponent<PlayerControl>();
        playerPrefix = (int)_playerControl.playerPrefix;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ItemSpawner")
        {
            _playerControl.ItemStateChange(true);
        }
        if (other.tag == "PowerUp")
        {
            _playerControl.IncreaseShootingLevel(1);
            Destroy(other.gameObject);
        }
        if (other.tag == "Heart")
        {
            _playerControl.HealDamage(1.0f);
            Destroy(other.gameObject);
        }
    }
    

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Chest")
        {
            if (Input.GetButtonDown("P" + playerPrefix + "_Action"))
            {
                other.GetComponent<TreasureChest>().OpenChest();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "ItemSpawner")
        {
            player.GetComponent<PlayerControl>().ItemStateChange(false);
        }
    }
}