using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawn : MonoBehaviour {

    private float lifetime;

	void Awake () {
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), .5f, Random.Range(-1f, 1f));
        rb.AddForce(dir * 500);
	}

    void Update ()
    {
        lifetime += Time.deltaTime;
    }
}
