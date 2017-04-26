﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    public GameObject orb;

    [HideInInspector]
    public bool activated;

    public Color activeColorInside;
    public Color activeColorOutside;
    private Color deactiveColorInside;
    private Color deactiveColorOutside;
    private Color activeColorEmission;
    private Renderer insideRend;
    private Renderer outsideRend;

    void Start()
    {
        insideRend = orb.GetComponent<Renderer>();
        outsideRend = GetComponent<Renderer>();

        deactiveColorInside = insideRend.material.color;
        deactiveColorOutside = outsideRend.material.color;

        activeColorEmission = activeColorOutside * Mathf.LinearToGammaSpace(0.8f);
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
        insideRend.material.color = activeColorInside;
        outsideRend.material.SetColor("_EmissionColor", activeColorEmission);
        outsideRend.materials[1].color = activeColorOutside;
    }

    public void Deactivate()
    {
        activated = false;
        insideRend.material.color = deactiveColorInside;
        outsideRend.materials[1].color = deactiveColorOutside;
    }
}


     