using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteMe : MonoBehaviour {
    public Animator Anime;
	// Use this for initialization
	void Start () {
        Anime = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1")){
            Debug.Log("toimii");
            Anime.Play("Sword", 0, 0.75f);
        }

    }
}
