using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

    public GameObject PowerUp;

	// Use this for initialization
	void Start () {
        SpawnPowerUp();
        SpawnPowerUp();
    }
	
	// Update is called once per frame
	void Update () {
        if (transform.childCount < 2)
        {
            SpawnPowerUp();
        }
	}

    private void SpawnPowerUp()
    {
        float x = transform.position.x + UnityEngine.Random.Range(-10.0f, 10.0f);
        float y = transform.position.y + 1;
        float z = transform.position.z + UnityEngine.Random.Range(-10.0f, 10.0f);
        Vector3 pos = new Vector3(x, y, z);
        var _powerUp = Instantiate(PowerUp, pos, transform.rotation);
        _powerUp.transform.parent = gameObject.transform;
    }
}
