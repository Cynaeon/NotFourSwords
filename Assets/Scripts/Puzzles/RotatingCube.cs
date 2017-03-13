using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCube : MonoBehaviour {

    public GameObject target;
    public ParticleSystem destroyEffect;
    public float rotateSpeed;

    private int timesRotated;
    private float Yrotation;
    private float Zrotation;

	// Use this for initialization
	void Start () {
        timesRotated = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (target.GetComponent<Target>().activated)
        {
            Activate();
        }
        
	}

    public void Activate()
    {
        if (timesRotated == 0)
        {
            Zrotation += rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, Zrotation);
            if (Zrotation >= 180)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
                target.GetComponent<Target>().Deactivate();
                timesRotated++;
            } 
        }
        else if (timesRotated == 1)
        {
            Yrotation += rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, Yrotation, Zrotation);
            if (Yrotation >= 270)
            {
                transform.rotation = Quaternion.Euler(0, 270, 180);
                target.GetComponent<Target>().Deactivate();
                timesRotated++;
            }
        }
        else if (timesRotated == 2)
        {
            Zrotation += rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, Yrotation, Zrotation);
            if (Zrotation >= 360)
            {
                transform.rotation = Quaternion.Euler(0, 270, 360);
                target.GetComponent<Target>().Deactivate();
                timesRotated++;
            }
        }
        else if (timesRotated == 3)
        {
            rotateSpeed += 1;
            Zrotation += rotateSpeed * Time.deltaTime;
            Yrotation += rotateSpeed * Time.deltaTime;
            transform.localScale -= Vector3.one * Time.deltaTime * 2;
            transform.rotation = Quaternion.Euler(0, Yrotation, Zrotation);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);

            if (transform.localScale.x < 0.1f)
            {
                Instantiate(destroyEffect, transform.position, transform.rotation);
                Destroy(gameObject);
            }

        }
    }
}
