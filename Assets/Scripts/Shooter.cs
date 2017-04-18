using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public GameObject bolt;
    public float firerate;

    private float currentFirerate;

	// Use this for initialization
	void Start () {
        currentFirerate = firerate;	
	}
	
	// Update is called once per frame
	void Update () {
        currentFirerate -= Time.deltaTime;
        if (currentFirerate <= 0)
        {
            Shoot();
            currentFirerate = firerate;
        }
	}

    private void Shoot()
    {
        Instantiate(bolt, transform.position, Quaternion.Euler(Vector3.forward));
    }
}
