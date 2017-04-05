using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTrail : MonoBehaviour {

    public float lifetime;
    public GameObject model;

    private Renderer _rend;
    private Color color;
 
	void Start () {
        _rend = model.GetComponent<Renderer>();
        color = _rend.material.color;
	}

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
