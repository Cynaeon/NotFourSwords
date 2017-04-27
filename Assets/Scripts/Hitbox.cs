using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    public GameObject player;

    private PlayerControl _playerControl;


    void Start()
    {
        _playerControl = player.GetComponent<PlayerControl>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fire")
        {
            Vector3 dir = other.transform.position - transform.position;
            _playerControl.TakeDamage(10, dir);
        }
        if (other.tag == "EnemyProjectile")
        {
            Vector3 dir = other.transform.position - transform.position;
            _playerControl.TakeDamage(1, dir);
            Destroy(other.gameObject);
        }
        if (other.tag == "Enemy")
        {
            Vector3 dir = other.transform.position - transform.position;
            _playerControl.TakeDamage(1, dir);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PeekabooArcher")
        {
            Vector3 dir = collision.transform.position - transform.position;
            _playerControl.TakeDamage(1, dir);
        }
    }
}
