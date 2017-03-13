using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleItemSpawner : MonoBehaviour {

    public GameObject item;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (item.transform.position.y <= -18)
        {
            Destroy(item);
            item  = Instantiate(item, transform.position, transform.rotation);
        }
	}
}
