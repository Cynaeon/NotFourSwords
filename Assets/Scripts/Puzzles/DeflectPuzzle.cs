using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectPuzzle : MonoBehaviour {

    public GameObject[] targets;
    public ParticleSystem spawnEffect;

    private int lastSpawnedTarget;

	void Start () {
        targets[0].SetActive(true);
        targets[1].SetActive(false);
        targets[2].SetActive(false);

        lastSpawnedTarget = 0;
	}

    void Update()
    {
        if (lastSpawnedTarget < 2)
        {
            if (targets[lastSpawnedTarget].GetComponent<Target>().activated)
            {
                lastSpawnedTarget++;
                targets[lastSpawnedTarget].SetActive(true);
                Instantiate(spawnEffect, targets[lastSpawnedTarget].transform.position, targets[1].transform.rotation);

            }
        }
    }
}
