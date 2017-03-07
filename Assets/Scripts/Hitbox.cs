using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    public GameObject player;

    private PlayerControl _playerControl;
    private string playerPrefix;

    void Start()
    {
        _playerControl = player.GetComponent<PlayerControl>();
        playerPrefix = _playerControl.playerPrefix;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fire")
        {
            _playerControl.TakeDamage(10.0f);
        }
        if (other.tag == "EnemyProjectile")
        {
            _playerControl.TakeDamage(1.0f);
        }
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
        if (other.tag == "Enemy")
        {
            _playerControl.TakeDamage(1.0f);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Chest")
        {
            if (Input.GetButtonDown(playerPrefix + "Action"))
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
