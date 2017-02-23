using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTrail : MonoBehaviour {

    public float lifetime;

    private Renderer _rend;
    private Color color;
 
	// Use this for initialization
	void Start () {
        _rend = GetComponent<Renderer>();
        color = _rend.material.color;
	}
	
	// Update is called once per frame
	void Update () {
        lifetime -= Time.deltaTime;
        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime / 2;
        }
        else
        {
            Destroy(gameObject);
        }
        _rend.material.color = new Color(color.r, color.g, color.b, lifetime);
	}
}
