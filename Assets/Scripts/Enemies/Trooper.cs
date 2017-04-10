using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trooper : Enemy {

    public float aggroRange;
    public float speed;

    public override void Start ()
    {
        base.Start();
    }

	public override void Update () {
        base.Update();

        Transform closestPlayer = FindClosestPlayer();
        float closestDist = Vector3.Distance(closestPlayer.position, transform.position);

        if (closestDist < aggroRange)
        {
            transform.LookAt(closestPlayer);
            transform.position = Vector3.MoveTowards(transform.position, closestPlayer.position, speed * Time.deltaTime);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            health -= 1;
            _rend.material.color = hitColor;
            startTime = Time.time;
        }
        if (other.tag == "PlayerProjectile")
        {
            other.transform.forward = Vector3.Reflect(other.transform.forward, transform.forward);
        }
    }
}
