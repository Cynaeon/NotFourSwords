﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeekabooArcher : Enemy {

    public float risingSpeed;
    public float shootingTime;
    public float aggroRange;
    public GameObject bolt;
    private float currentTime;
    public Transform shootPos;
    private Vector3 hidePosition;
    private Vector3 upPosition;

    public override void Start() {
        base.Start();
        hidePosition = transform.position;
        upPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }

    public override void Update() {
        base.Update();

        Transform closestPlayer = FindClosestPlayer();
        float closestDist = Vector3.Distance(closestPlayer.position, transform.position);

        if (closestDist <= aggroRange)
        {
            Vector3 target = new Vector3(closestPlayer.position.x, transform.position.y, closestPlayer.position.z);
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, upPosition, risingSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, hidePosition, risingSpeed * Time.deltaTime);
        }
        if (transform.position == upPosition)
        {
            currentTime += Time.deltaTime;
            
            if (currentTime >= shootingTime)
            {
                Shoot();
                currentTime = 0;
            }
        }
        else
        {
            currentTime = 0;
        }
    }

    private void Shoot()
    {
        Vector3 ShootPosition = new Vector3(shootPos.position.x, shootPos.position.y, shootPos.position.z);
        Instantiate(bolt, ShootPosition, transform.rotation);
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
            health -= 1;
            _rend.material.color = hitColor;
            startTime = Time.time;
        }
    }
}
