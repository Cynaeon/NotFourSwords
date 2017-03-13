using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSwitch : MonoBehaviour {

    public GameObject orb;
    public GameObject particles;

    [HideInInspector] public bool activated;

    private Color activeColor;
    private Color deactiveColor;
    private Renderer _rend;

    // Use this for initialization
    void Start () {
        _rend = orb.GetComponent<Renderer>();
        activeColor = new Color(1, 0.5f, 0.5f, 1);
        deactiveColor = _rend.material.color;
        particles.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerProjectile")
        {
            if (activated)
            {
                Deactivate();
            }
            else
            {
                Activate();
            }
        } 
    }

    private void Activate()
    {
        activated = true;
        _rend.material.color = activeColor;
        particles.SetActive(true);
    }

    private void Deactivate()
    {
        activated = false;
        _rend.material.color = deactiveColor;
        particles.SetActive(false);
    }
}
