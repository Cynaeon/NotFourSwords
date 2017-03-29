using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour {

	public float speed;
	public float lifeTime;
    public GameObject fire;

    public bool onFire;

    private bool hitWall;

    public enum projectileType
    {
        Enemy, 
        Player
    }

    public projectileType type;

    void Start()
    {
        if (fire)
        {
            fire.SetActive(false);
        }
    }

	void Update () {
        if (!hitWall)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        
		lifeTime -= Time.deltaTime;

		if (lifeTime < 0) {
			Destroy (gameObject);
		}

        if (onFire)
        {
            fire.SetActive(true);
        }
        if (hitWall)
        {

        }
	}

	void OnTriggerEnter(Collider other) {
        if (type == projectileType.Player)
        {
            if (other.tag == "Enemy")
            {
                Destroy(gameObject);
            }
            if (other.tag == "Shield")
            {
                Destroy(gameObject);
            }
        }
        if (type == projectileType.Enemy)
        {
            if (other.tag == "Player")
            {
                Destroy(gameObject);
            }
        }
        
        if (other.tag == "Walls" || other.tag == "PushBlock")
        {
            hitWall = true;
        }
        if (other.tag == "Fire")
        {
            onFire = true;
        }
    }
}
