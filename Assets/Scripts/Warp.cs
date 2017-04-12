using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour {

    public Transform destination;

	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 exitPoint = destination.position + destination.forward * 3;
            other.transform.position = exitPoint;
            other.transform.rotation = destination.rotation;
        }
    }
}
