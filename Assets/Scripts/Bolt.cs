using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour {

	public float speed;
	public float lifeTime;

    public enum projectileType
    {
        Enemy, 
        Player
    }

    public projectileType type;
	
	void Update () {
		transform.Translate (Vector3.forward * Time.deltaTime * speed);
		lifeTime -= Time.deltaTime;

		if (lifeTime < 0) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
        if (type == projectileType.Player)
        {
            if (other.tag == "Enemy")
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
    }
}
