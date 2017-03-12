using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{

    public float Xspeed;
    public float Yspeed;
    public float Zspeed;
    Quaternion rotation;

    private void Awake()
    {
        rotation = transform.rotation;
    }

    private void Update()
    {
        transform.rotation = rotation;
    }

    void LateUpdate()
    {
        transform.rotation = rotation;
        // Rotate the object around its local X axis at 1 degree per second
        transform.Rotate(Vector3.right * Time.deltaTime * Xspeed);

        // ...also rotate around the World's Y axis
        transform.Rotate(Vector3.up * Time.deltaTime * Yspeed);

        transform.Rotate(Vector3.forward * Time.deltaTime * Zspeed);
        rotation = transform.rotation;
    }
}
