using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBreak : MonoBehaviour
{

    public GameObject BreakAnimation;


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            Instantiate(BreakAnimation, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
