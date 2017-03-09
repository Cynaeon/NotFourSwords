using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health = 10.0f;
    public ParticleSystem hitEffect;

    private Renderer _rend;
    private Color defaultColor;
    private Color hitColor = Color.red;
    private float startTime;

	public virtual void Start () {
        _rend = GetComponent<Renderer>();
        defaultColor = _rend.material.color;
	}
	
	public virtual void Update () {
		if (health <= 0)
        {
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if (_rend.material.color != defaultColor)
        {
            _rend.material.color = Color.Lerp(hitColor, defaultColor, Time.time - startTime);
        }
	}

    public virtual Transform FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDist = 0;
        Transform closestPlayer = null;

        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (closestDist == 0)
            {
                closestDist = dist;
                closestPlayer = player.transform;
            }
            else if (closestDist > dist)
            {
                closestDist = dist;
                closestPlayer = player.transform;
            }
        }
        return closestPlayer;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerProjectile" || other.tag == "Sword")
        {
            health -= 1;
            _rend.material.color = hitColor;
            startTime = Time.time;
        }
    }
}
