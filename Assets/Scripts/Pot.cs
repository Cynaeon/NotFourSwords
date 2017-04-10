using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour {

    public enum Pickups
    {
        Heart,
        smallMana,
        bigMana
    }

    public Pickups contains;
    public int amount;

    public GameObject heart;
    public GameObject smallMana;
    public GameObject bigMana;
    public GameObject breakAnimation;

    private GameObject itemDrop;
    private Quaternion rot;

    public void Start()
    {
        if (contains == Pickups.Heart)
        {
            rot = Quaternion.Euler(-90, 0, 0);
            itemDrop = heart;
        }
        else if (contains == Pickups.smallMana)
        {
            rot = Quaternion.Euler(0, 0, 0);
            itemDrop = smallMana;
        }
        else if (contains == Pickups.bigMana)
        {
            rot = Quaternion.Euler(0, 0, 0);
            itemDrop = bigMana;
        }
    }

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
        for (int i = 0; i < amount; i++)
        {
            Instantiate(itemDrop, pos, rot);
        }
        Instantiate(breakAnimation, transform.position, (Quaternion.Euler(0f, Random.Range(0.0f, 360.0f),0f)));
        Destroy(gameObject);
    }
}
