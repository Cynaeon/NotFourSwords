using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour {

    public float health;
    public GameObject heart;
	
	void Update () {
		if (health <= 0)
        {
            Destroy();
        }
	}

    public void Destroy()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        Instantiate(heart, pos, transform.rotation);
        Instantiate(heart, pos, transform.rotation);
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerProjectile")
        {
            health -= 1;
        }
    }
}
