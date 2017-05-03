using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBox : MonoBehaviour
{

    public GameObject player;

    private GameObject gameManager;
    private PlayerControl _playerControl;
    private int playerPrefix;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
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
            _playerControl.HealDamage(1);
            Destroy(other.gameObject);

        }
        if(other.tag == "Mana")
        {
            int value = other.gameObject.GetComponent<Mana>().ManaValue;
            _playerControl.IncreaseScore(value);
            Destroy(other.gameObject);
            gameManager.GetComponent<GameManager>().TotalScore(value);
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