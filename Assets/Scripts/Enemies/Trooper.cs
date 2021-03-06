﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trooper : Enemy {

    public float aggroRange;
    public float speed;
    private Animator anime;

    private Rigidbody _rb;

    public override void Start ()
    {
        _rb = GetComponent<Rigidbody>();
        anime = GetComponent<Animator>();
        base.Start();
    }

	public override void Update () {
        base.Update();

        Transform closestPlayer = FindClosestPlayer();
        float closestDist = Vector3.Distance(closestPlayer.position, transform.position);

        if (closestDist < aggroRange)
        {
            anime.SetBool("InRange", true);
            var lookPos = closestPlayer.position - transform.position;
            lookPos.y = 0;
            if (lookPos != Vector3.zero)
            {
                var rotation = Quaternion.LookRotation(lookPos);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
            }
            Vector3 target = new Vector3(closestPlayer.position.x, transform.position.y, closestPlayer.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }else
        {

            anime.SetBool("InRange", false);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            Vector3 dir = transform.position - other.transform.position;
            dir = dir.normalized;
            dir.y = 0.5f;
            _rb.AddForce(dir * 10000);
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
