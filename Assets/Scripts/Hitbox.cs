using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

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
        if (other.tag == "Fire")
        {
            _playerControl.TakeDamage(10.0f);
        }
        if (other.tag == "EnemyProjectile")
        {
            _playerControl.TakeDamage(1.0f);
        }
        if (other.tag == "Enemy")
        {
            _playerControl.TakeDamage(1.0f);
        }
    }
}
