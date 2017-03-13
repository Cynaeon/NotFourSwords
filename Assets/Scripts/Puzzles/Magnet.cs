using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour {

    public float distance;
    public float magnetVelocity;
    public GameObject[] activators;

    public enum ActivatorsRequired
    {
        All = 0,
        JustOne = 1
    }

    public ActivatorsRequired activatorsRequired;

    private bool activated;
    ParticleSystem particles;
    Renderer particlesRend;

    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        if (particles)
        {
            var ma = particles.main;
            ma.startLifetime = distance / 30;
            particlesRend = particles.GetComponent<Renderer>();
            particlesRend.enabled = false;
        }
    }
	
	void Update () {

        activated = CheckActivators();

        if (activated)
        {
            particlesRend.enabled = true;
            RaycastHit hit;
            Vector3 forward = transform.TransformDirection(Vector3.forward) * distance;
            Debug.DrawRay(transform.position, forward, Color.green);
            if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
            {
                if (hit.collider.tag == "Metallic")
                {
                    hit.transform.position = Vector3.MoveTowards(hit.transform.position, transform.position, magnetVelocity * Time.deltaTime);
                }
            }
        }
        else
        {
            particlesRend.enabled = false;
        }
    }

    private bool CheckActivators()
    {
        foreach (GameObject activator in activators)
        {
            if (activator.GetComponent<PressurePlate>())
            {
                if (activator.GetComponent<PressurePlate>().activated)
                {
                    if ((int)activatorsRequired == 1)
                    {
                        return true;
                    }
                }
                else if ((int)activatorsRequired == 0)
                {
                    return false;
                }
            }
            if (activator.GetComponent<HitSwitch>())
            {
                if (activator.GetComponent<HitSwitch>().activated)
                {
                    if ((int)activatorsRequired == 1)
                    {
                        return true;
                    }
                }
                else if ((int)activatorsRequired == 0)
                {
                    return false;
                }
            }
        }

        if ((int)activatorsRequired == 0)
        {
            return true;
        }
        return false;
    }
}
