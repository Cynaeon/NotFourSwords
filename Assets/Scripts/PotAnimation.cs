using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotAnimation : MonoBehaviour {
    public float dropSpeed;
	// Use this for initialization
	void Start () {
        dropSpeed = 0.15f;
        Invoke("Destroy", 5f);
	}
	
	// Update is called once per frame
	void Update () {

        transform.Translate(Vector3.down * Time.deltaTime * dropSpeed);
    }

    void Destroy()
    {
        Destroy(this.gameObject);
    }
}
