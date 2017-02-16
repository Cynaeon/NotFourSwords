using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleParticles : MonoBehaviour {

    public Transform invisibleBlock;
    

	// Use this for initialization
	void Start () {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var sh = ps.shape;
        sh.box = new Vector3(invisibleBlock.transform.localScale.x + 0.5f, invisibleBlock.transform.localScale.z + 0.5f, 1);
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
