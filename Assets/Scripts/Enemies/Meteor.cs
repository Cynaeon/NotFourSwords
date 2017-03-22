using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {

    public Transform crosshair;
    public float speed;

	void Update () {
        transform.position += Vector3.down * Time.deltaTime * speed;
        crosshair.transform.position += Vector3.up * Time.deltaTime * speed;

        if (transform.position.y < crosshair.position.y)
        {
            Destroy(gameObject);
        }
	}
}
