using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour {

	public float speed;
	public float lifeTime;
	public ParticleSystem hitEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward * Time.deltaTime * speed);
		lifeTime -= Time.deltaTime;

		if (lifeTime < 0) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy") {
			Instantiate (hitEffect, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}
}
