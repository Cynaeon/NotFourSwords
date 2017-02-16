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
    private Vector3 hidePosition;
    private Vector3 upPosition;

    public override void Start() {
        base.Start();
        hidePosition = transform.position;
        upPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }

    public override void Update() {
        base.Update();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");
        float closestDist = 0;
        Transform closestPlayer = null;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, transform.position);
            if (closestDist == 0)
            {
                closestDist = dist;
                closestPlayer = enemy.transform;
            }
            else if (closestDist > dist)
            {
                closestDist = dist;
                closestPlayer = enemy.transform;
            }
        }

        if (closestDist <= aggroRange)
        {
            Vector3 target = new Vector3(closestPlayer.position.x, transform.position.y, closestPlayer.position.z);
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, upPosition, risingSpeed * Time.deltaTime);
        } else
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
        Instantiate(bolt, transform.position, transform.rotation);
    }
}
