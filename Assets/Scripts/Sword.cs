using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    
    public float lifeTime;
    private float curLifeTime;

    void OnEnable()
    {
        curLifeTime = lifeTime;
    }

	void Update () {
        
        curLifeTime -= Time.deltaTime;
        if (curLifeTime <= 0)
        {
            gameObject.SetActive(false);
        }
	}
}
