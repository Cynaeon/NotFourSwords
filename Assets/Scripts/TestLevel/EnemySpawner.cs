using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemy;
    public GameObject flyingEnemy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        Vector3 enemyPos1 = new Vector3(-21, 1, 21);
        Vector3 enemyPos2 = new Vector3(-35, 1, 30);
        //Vector3 flyingEnemyPos = new Vector3(-39, 2, 37);
        Instantiate(enemy, enemyPos1, Quaternion.identity);
        Instantiate(enemy, enemyPos2, Quaternion.identity);
        //Instantiate(flyingEnemy, flyingEnemyPos, Quaternion.identity);
    }
}
