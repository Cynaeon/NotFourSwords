using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour {
    
    public GameObject heart;
    private Animator anime;
    private bool broken;
    private float dropSpeed;

    void Start()
    {
        anime = GetComponentInChildren<Animator>();
        dropSpeed = 0.2f; 
    }

    void Update()
    {
        if (broken)
        {
            transform.Translate(Vector3.down * Time.deltaTime * dropSpeed);
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerProjectile")
        {
            Destroy();

            Destroy(other.gameObject);
        }
    }


    public void Destroy()
    {
        Destroy(gameObject.GetComponent<CapsuleCollider>());
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        Instantiate(heart, pos, transform.rotation);
        Instantiate(heart, pos, transform.rotation);
        anime.Play("Break");
        Invoke("reallyDestroy", 4);
        broken = true;
    }

    public void reallyDestroy()
    {
        Destroy(gameObject);
    }

}
