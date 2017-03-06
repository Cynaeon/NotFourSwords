using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawn : MonoBehaviour {

	void Awake () {
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 dir = new Vector3(Random.Range(-1, 1), 1, Random.Range(-1, 1));
        rb.AddForce(dir * 700);
	}
}
