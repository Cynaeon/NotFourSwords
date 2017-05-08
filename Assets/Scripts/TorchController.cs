using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour {

    public GameObject go_torch1;
    public GameObject go_torch2;

    private Torch torch1;
    private Torch torch2;

	// Use this for initialization
	void Start () {
        torch1 = go_torch1.GetComponent<Torch>();
        torch2 = go_torch2.GetComponent<Torch>();
        torch1.lit = false;
        torch2.lit = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            torch1.lit = true;
            torch2.lit = true;
        }
    }
}
