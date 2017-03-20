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
            Vector3 dir = other.transform.position - transform.position;
            _playerControl.TakeDamage(10.0f, dir);
        }
        if (other.tag == "EnemyProjectile")
        {
            Vector3 dir = other.transform.position - transform.position;
            _playerControl.TakeDamage(1.0f, dir);
            Destroy(other.gameObject);
        }
        if (other.tag == "Enemy")
        {
            Vector3 dir = other.transform.position - transform.position;
            _playerControl.TakeDamage(1.0f, dir);
        }
    }
}
