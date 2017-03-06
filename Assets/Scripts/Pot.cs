using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour {
    
    public GameObject heart;
    private Animator anime;

    void Start()
    {
        anime = GetComponentInChildren<Animator>();
    }

    public void Destroy()
    {
        Destroy(gameObject.GetComponent<CapsuleCollider>());
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        Instantiate(heart, pos, transform.rotation);
        Instantiate(heart, pos, transform.rotation);
        anime.Play("Break");
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerProjectile")
        {
            Destroy();
        }
    }
}
