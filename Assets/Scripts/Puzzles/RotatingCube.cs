using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCube : MonoBehaviour {

    public GameObject target;
    public GameObject cannon;
    public GameObject meteor;
    public ParticleSystem destroyEffect;
    public float rotateSpeed;
    public float meteorFireRate;

    private float lastMeteor;
    private int timesRotated;
    private float Yrotation;
    private float Zrotation;
    private bool increasedSpeed;

    void Start() {
        timesRotated = 0;
    }

    void Update() {
        DefenceMechanism();
        if (target.GetComponent<Target>().activated)
        {
            Activate();
        }
    }

    public void Activate()
    {
        if (timesRotated == 0)
        {
            Zrotation += rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, Zrotation);
            if (Zrotation >= 180)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
                target.GetComponent<Target>().Deactivate();
                timesRotated++;
            }
        }
        else if (timesRotated == 1)
        {
            Yrotation += rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, Yrotation, Zrotation);
            if (Yrotation >= 270)
            {
                transform.rotation = Quaternion.Euler(0, 270, 180);
                target.GetComponent<Target>().Deactivate();
                timesRotated++;
            }
        }
        else if (timesRotated == 2)
        {
            Zrotation += rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, Yrotation, Zrotation);
            if (Zrotation >= 360)
            {
                transform.rotation = Quaternion.Euler(0, 270, 360);
                target.GetComponent<Target>().Deactivate();
                timesRotated++;
            }
        }
        else if (timesRotated >= 3)
        {
            timesRotated = 4;
            rotateSpeed += 1;
            Zrotation += rotateSpeed * Time.deltaTime;
            Yrotation += rotateSpeed * Time.deltaTime;
            transform.localScale -= Vector3.one * Time.deltaTime * 2;
            transform.rotation = Quaternion.Euler(0, Yrotation, Zrotation);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);

            if (transform.localScale.x < 0.1f)
            {
                Instantiate(destroyEffect, transform.position, transform.rotation);
                Destroy(gameObject);
                Destroy(cannon);
            }
        }
    }

    private void DefenceMechanism()
    {
        if (timesRotated != 4)
        {
            if (timesRotated >= 1)
            {
                // Start firing meteors
                lastMeteor += Time.deltaTime;
                if (lastMeteor > meteorFireRate)
                {
                    Transform player = FindRandomPlayer();
                    Vector3 spawnPoint = new Vector3(player.position.x, 30, player.position.z);
                    Instantiate(meteor, spawnPoint, Quaternion.identity);
                    lastMeteor = 0;
                }
            }
            if (timesRotated >= 2)
            {
                // Start firing the spinning cannon
                cannon.GetComponent<RotatingCubeCannon>().StartFiring();
            }
            if (timesRotated == 3)
            {
                // Speed things up
                IncreaseSpeed();
            }
        }
        else
        {
            cannon.GetComponent<RotatingCubeCannon>().StopFiring();
        }
    }

    private void IncreaseSpeed()
    {
        if (!increasedSpeed)
        {
            meteorFireRate /= 2;
            cannon.GetComponent<RotatingCubeCannon>().IncreaseSpeed();
            increasedSpeed = true;
        }
    }

    public virtual Transform FindRandomPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        return players[UnityEngine.Random.Range(0, players.Length)].transform;
    }
}
