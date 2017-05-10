using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflect : MonoBehaviour {

    public Color flashColor;
    public float flashSpeed;

    private Color startEmission;
    private Material material;
    private float time;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        startEmission = material.GetColor("_EmissionColor");
    }

    void Update()
    {
        if (time > 0)
        {
            Color lerpedColor = Color.Lerp(startEmission, flashColor, time);
            material.SetColor("_EmissionColor", lerpedColor);
            time -= Time.deltaTime * flashSpeed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerProjectile")
        {
            other.transform.forward = Vector3.Reflect(other.transform.forward, transform.forward);
            material.SetColor("_EmissionColor", flashColor);
            time = 1;
        }
    } 
}
