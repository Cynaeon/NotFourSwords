using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

    public float swingSpeed;
    public float lifeTime;

    private float startPoint;
    private Quaternion startRot;
    private float curLifeTime;

    void Start()
    {
        startRot = new Quaternion(transform.rotation.x, 0, 0, transform.rotation.w);
    }

    void OnEnable()
    {
        startPoint = -180;
        transform.rotation = startRot;
        curLifeTime = lifeTime;
    }

	void Update () {
        startPoint += Time.deltaTime * swingSpeed;
        transform.localRotation = Quaternion.Euler(startPoint, 0, 0);
        
        curLifeTime -= Time.deltaTime;
        if (curLifeTime <= 0)
        {
            gameObject.SetActive(false);
        }
	}
}
