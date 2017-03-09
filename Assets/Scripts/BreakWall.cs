using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : MonoBehaviour {

    public GameObject wall;
    public GameObject brokenPieces;

    private Renderer[] _rends;
    private Collider _collider;
    private float a;

	void Start () {
        _collider = GetComponent<BoxCollider>();
        _rends = brokenPieces.GetComponentsInChildren<Renderer>();
        a = _rends[0].material.color.a;
	}
	
	void Update () {
		if (brokenPieces.activeSelf)
        {
            a -= Time.deltaTime / 2;
            foreach (Renderer renderer in _rends)
            {
                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, a);
            }
            if (a <= 0)
            {
                Destroy(gameObject);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            brokenPieces.SetActive(true);
            Destroy(wall);
        }
    }
}
