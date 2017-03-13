using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    [HideInInspector] public bool activated;
    private Color activeColor;
    private Color deactiveColor;
    private Renderer _rend;

    // Use this for initialization
    void Start()
    {
        _rend = GetComponent<Renderer>();
        activeColor = new Color(1, 0.5f, 0.5f, 1);
        deactiveColor = _rend.material.color;
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerProjectile")
        {
            Activate(); 
        }
    }

    private void Activate()
    {
        activated = true;
        _rend.material.color = activeColor;
    }

    public void Deactivate()
    {
        activated = false;
        _rend.material.color = deactiveColor;
    }
}
