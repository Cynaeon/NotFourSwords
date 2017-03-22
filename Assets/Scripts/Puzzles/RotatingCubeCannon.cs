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

	void Start () {
        yRot = 45;
	}
	
	void Update () {
        lastShot += Time.deltaTime;
		if (firing)
        {
            yRot += rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRot, transform.rotation.eulerAngles.z);

            if (lastShot >= fireRate)
            {
                Instantiate(bolt, cannonHead1.transform.position, cannonHead1.transform.rotation);
                Instantiate(bolt, cannonHead2.transform.position, cannonHead2.transform.rotation);
                lastShot = 0;
            }
        }
    }

    public void StartFiring() 
    {
        firing = true;
    }

    public void StopFiring()
    {
        firing = false;
    }

    public void IncreaseSpeed()
    {
        rotateSpeed *= 2;
    }
}
