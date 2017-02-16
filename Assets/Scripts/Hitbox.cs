using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    public GameObject player;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fire")
        {
            player.GetComponent<PlayerControl>().TakeDamage(10.0f);
        }
        if (other.tag == "EnemyProjectile")
        {
            player.GetComponent<PlayerControl>().TakeDamage(10.0f);
        }
        if (other.tag == "ItemSpawner")
        {
            player.GetComponent<PlayerControl>().ItemStateChange(true);
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
