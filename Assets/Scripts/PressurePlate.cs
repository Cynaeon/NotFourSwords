using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

    public Transform plate;
    [HideInInspector] public bool activated;

    private Vector3 upPos;
    private Vector3 downPos;
    private Renderer _rend;
    private Color activeColor;
    private Color deactiveColor;

	void Start () {
        upPos = plate.position;
        downPos = new Vector3(plate.position.x, plate.position.y - 0.25f, plate.position.z);
        _rend = plate.GetComponent<Renderer>();
        activeColor = new Color(1, 0.5f, 0.5f, 1);
        deactiveColor = _rend.material.color;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "PushBlock")
        {
            StartCoroutine("Activate");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "PushBlock")
        {
            StartCoroutine("Deactivate");
        }
    }

    private IEnumerator Activate()
    {
        activated = true;
        _rend.material.color = activeColor;
        while (Vector3.Distance(plate.position, downPos) > 0.01f)
        {
            plate.position = Vector3.MoveTowards(plate.position, downPos, Time.deltaTime * 1);
            yield return null;
        }
    }

    private IEnumerator Deactivate()
    {
        activated = false;
        _rend.material.color = deactiveColor;
        while (Vector3.Distance(plate.position, upPos) > 0.01f)
        {
            plate.position = Vector3.MoveTowards(plate.position, upPos, Time.deltaTime * 1);
            yield return null;
        }
    }
}
