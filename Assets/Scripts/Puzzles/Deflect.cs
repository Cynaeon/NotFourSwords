using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerProjectile")
        {
            other.transform.forward = Vector3.Reflect(other.transform.forward, transform.forward);
        }
    } 
}
