using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawn : MonoBehaviour {

    [HideInInspector] public bool collected;
    public float lifetime;
    [HideInInspector] public bool canBeCollected;

    private float currLifetime;
    private Collider collider;
    private Material material;
    private Color defaultColor;

	void Awake () {
        canBeCollected = false;
        material = GetComponentInChildren<Renderer>().material;
        defaultColor = material.color;
        collider = GetComponent<Collider>();
        collider.enabled = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), .5f, Random.Range(-1f, 1f));
        rb.AddForce(dir * 500);
	}

    void Update ()
    { 
        if (currLifetime * 1.5f >= lifetime)
        {
            material.color = Color.Lerp(defaultColor, Color.clear, Mathf.PingPong(Time.time * 3, 1));
        }
        currLifetime += Time.deltaTime;
        if (currLifetime > 0.5f)
        {
            canBeCollected = true;
            collider.enabled = true;
        }
        if (currLifetime >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
