using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour {

    //public Transform lid;
    public GameObject powerUp;

    private bool opened;
    private Animator anime;

	// Use this for initialization
	void Start () {
        anime = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (opened)
        {
            //lid.transform.eulerAngles = Vector3.Lerp(lid.rotation.eulerAngles, openPos, Time.deltaTime);
        }
	}

    public void OpenChest()
    {
        if (!opened)
        {
            anime.Play("chestOpen");
            opened = true;
            Invoke("SpawnStuff", 1);
            Invoke("SpawnStuff", 1.3f);
            Invoke("SpawnStuff", 1.7f);
            Invoke("SpawnStuff", 2.1f);
        }
    }

    private void SpawnStuff()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Instantiate(powerUp, pos, transform.rotation);
    }
}
