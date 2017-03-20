using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour {
    
    public GameObject heart;
    public GameObject breakAnimation;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerProjectile" )
        {

            Break();
            Destroy(other.gameObject);
        }

        if (other.tag == "Sword")
        {
            Break();
        }
    }


    public void Break()
    {
        Destroy(gameObject.GetComponent<CapsuleCollider>());
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        Quaternion rot = Quaternion.Euler(-90, 0, 0);
        Instantiate(heart, pos, rot);
        Instantiate(breakAnimation, transform.position, (Quaternion.Euler(0f, Random.Range(0.0f, 360.0f),0f)));
        Destroy(gameObject);
    }
}
