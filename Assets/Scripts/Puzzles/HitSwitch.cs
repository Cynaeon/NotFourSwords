using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSwitch : MonoBehaviour {

    public GameObject orb;
    public GameObject hitEffect;

    [HideInInspector] public bool activated;

    private Color activeColorInside;
    private Color activeColorOutside;
    private Color deactiveColorInside;
    private Color deactiveColorOutside;
    private Renderer insideRend;
    private Renderer outsideRend;

    void Start () {
        insideRend = orb.GetComponent<Renderer>();
        outsideRend = GetComponent<Renderer>();
        
        deactiveColorInside = insideRend.material.color;
        deactiveColorOutside = outsideRend.material.color;
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
            Destroy(other.gameObject);
        } 
    }

    private void Activate()
    {
        hitEffect.SetActive(true);
        activated = true;
        insideRend.material.color = activeColorInside;
        outsideRend.material.color = activeColorOutside;
    }

    private void Deactivate()
    {
        hitEffect.SetActive(false);
        activated = false;
        insideRend.material.color = deactiveColorInside;
        outsideRend.material.color = deactiveColorOutside;
    }
}
