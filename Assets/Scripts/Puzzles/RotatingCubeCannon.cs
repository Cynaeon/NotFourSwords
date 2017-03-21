using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCubeCannon : MonoBehaviour {

    public GameObject bolt;
    public Transform cannonHead1;
    public Transform cannonHead2;
    public float fireRate;
    public float rotateSpeed;

    private float lastShot;
    private bool firing;
    private float yRot;

	// Use this for initialization
	void Start () {
        firing = true;
        yRot = 45;
	}
	
	// Update is called once per frame
	void Update () {
        lastShot += Time.deltaTime;
		if (firing && lastShot >= fireRate)
        {
            Instantiate(bolt, cannonHead1.transform.position, cannonHead1.transform.rotation);
            Instantiate(bolt, cannonHead2.transform.position, cannonHead2.transform.rotation);
            lastShot = 0;
        }
        yRot += rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRot, transform.rotation.eulerAngles.z);
    }
}
