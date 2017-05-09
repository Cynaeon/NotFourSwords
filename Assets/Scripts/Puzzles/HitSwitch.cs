using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSwitch : MonoBehaviour {

    public GameObject orb;
    public GameObject hitEffect;
    public GameObject impactEffect;

    [HideInInspector] public bool activated;

    public Color activeColorInside;
    public Color activeColorOutside;
    public Color activeEmissionInside;
    public Color activeEmissionOutside;

    private Color deactiveColorInside;
    private Color deactiveColorOutside;
    private Color deactiveEmissionOutside;
    private Color deactiveEmissionInside;
    private Renderer insideRend;
    private Renderer outsideRend;

    void Start () {
        insideRend = orb.GetComponent<Renderer>();
        outsideRend = GetComponent<Renderer>();
        
        deactiveColorInside = insideRend.material.color;
        deactiveColorOutside = outsideRend.material.color;
        deactiveEmissionOutside = outsideRend.material.GetColor("_EmissionColor");
        deactiveEmissionInside = insideRend.material.GetColor("_EmissionColor");
        activeColorInside = new Color(0.9f, 0.9f, 0.9f, insideRend.material.color.a);
        activeColorOutside = new Color(1, 1, 1, outsideRend.material.color.a);
        
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
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(other.gameObject);
        } 
    }

    private void Activate()
    {
        hitEffect.SetActive(true);
        activated = true;
        insideRend.material.color = activeColorInside;
        insideRend.material.SetColor("_EmissionColor", activeEmissionInside);
        outsideRend.material.color = activeColorOutside;
        outsideRend.material.SetColor("_EmissionColor", activeEmissionOutside);
    }

    private void Deactivate()
    {
        hitEffect.SetActive(false);
        activated = false;
        insideRend.material.color = deactiveColorInside;
        insideRend.material.SetColor("_EmissionColor", deactiveEmissionInside);
        outsideRend.material.color = deactiveColorOutside;
        outsideRend.material.SetColor("_EmissionColor", deactiveEmissionOutside);
    }
}
