using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumBox : MonoBehaviour {

    public float pullSpeed;

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Heart" || other.tag == "Mana")
        {
            other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position, Time.deltaTime * pullSpeed);
        }
    }
}
