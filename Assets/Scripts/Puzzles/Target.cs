using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    public GameObject orb;
    public ParticleSystem hitEffect;

    [HideInInspector]
    public bool activated;

    public Color activeColorInside;
    public Color activeColorOutside;
    public Color activeEmissionInside;
    public Color activeEmissionOutside;
    private Color deactiveColorInside;
    private Color deactiveColorOutside;
    private Renderer insideRend;
    private Renderer outsideRend;

    void Start()
    {
        insideRend = orb.GetComponent<Renderer>();
        outsideRend = GetComponent<Renderer>();

        deactiveColorInside = insideRend.material.color;
        deactiveColorOutside = outsideRend.material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerProjectile")
        {
            if (!activated)
            {
                Activate();
            }
        }
    }

    private void Activate()
    {
        activated = true;
        Instantiate(hitEffect, transform.position, transform.rotation);
        insideRend.material.color = activeColorInside;
        insideRend.material.SetColor("_EmissionColor", activeEmissionInside);
        outsideRend.materials[1].color = activeColorOutside;
        outsideRend.materials[1].SetColor("_EmissionColor", activeEmissionOutside);
        
    }

    public void Deactivate()
    {
        activated = false;
        insideRend.material.color = deactiveColorInside;
        outsideRend.materials[1].color = deactiveColorOutside;
    }
}


     