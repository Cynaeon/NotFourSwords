using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWall : MonoBehaviour {

    public GameObject activator;
    public float turnSpeed;

    private float yRot;
    private bool activated;

	void Start () {
        yRot = transform.localRotation.eulerAngles.y;
	}
	
	void Update () {
    	activated = activator.GetComponent<HitSwitch>().activated;
        if (activated)
        {
            if (yRot >= -90)
            {
                yRot -= Time.deltaTime * turnSpeed;
            }
        }
        else
        {
            if (yRot <= 0)
            {
                yRot += Time.deltaTime * turnSpeed;
            }
        }
        transform.localRotation = Quaternion.Euler(0, yRot, 0);
    }
}
