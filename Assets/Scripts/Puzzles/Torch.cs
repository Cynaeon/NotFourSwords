using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {

    public bool lit;
    public GameObject fire;

	void Update () {
		if (lit)
        {
            fire.SetActive(true);
        }
        else
        {
            fire.SetActive(false);
        }
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "PlayerProjectile")
        {
            if (other.GetComponent<Bolt>().onFire)
            {
                lit = true;
            }
        }
    }
}
