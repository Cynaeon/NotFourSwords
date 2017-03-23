using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public GameObject[] activators;
    public enum ActivatorsRequired
    {
        All = 0,
        JustOne = 1
    }

    public float speed;
    public Vector3 endPos;
    public ActivatorsRequired activatorsRequired;

    private Vector3 startPos;
    private bool activated;

    void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(transform.position.x + endPos.x, transform.position.y + endPos.y, transform.position.z + endPos.z);
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
            if (activator.GetComponent<Target>())
            {
                if (activator.GetComponent<Target>().activated)
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
        while (Vector3.Distance(transform.position, endPos) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * speed);
            yield return null;
        }
    }

    public IEnumerator Deactivate()
    {
        while (Vector3.Distance(transform.position, startPos) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime * speed);
            yield return null;
        }
    }
}

