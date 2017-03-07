using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

    public Transform bar;
    public GameObject[] activators;
    public enum ActivatorsRequired
    {
        All = 0,
        JustOne = 1
    }

    public ActivatorsRequired activatorsRequired;

    private Vector3 openPos;
    private Vector3 closePos;
    private bool activated;

	void Start () {
        closePos = transform.position;
        openPos = new Vector3(bar.position.x, bar.position.y + 8, bar.position.z);
	}

    void Update()
    {
        activated = CheckActivators();
        
        if (activated)
        {
            StartCoroutine("Activate");
        }
        else
        {
            StartCoroutine("Deactivate");
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

    public IEnumerator Activate()
    {
        while (Vector3.Distance(bar.position, openPos) > 0.01f)
        {
            bar.position = Vector3.MoveTowards(bar.position, openPos, Time.deltaTime * 10);
            yield return null;
        }
    }

    public IEnumerator Deactivate()
    {
        while (Vector3.Distance(bar.position, closePos) > 0.01f)
        {
            bar.position = Vector3.MoveTowards(bar.position, closePos, Time.deltaTime * 10);
            yield return null;
        }
    }
}
