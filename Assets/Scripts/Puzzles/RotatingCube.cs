using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCube : MonoBehaviour {

    public GameObject target;
    public GameObject cannon;
    public GameObject meteor;
    public GameObject ladder;
    public ParticleSystem destroyEffect;
    public float rotateSpeed;
    public float meteorFireRate;

    public GameObject box;
    public Material sleeping;
    public Material angry;
    public Material hurt;

    private float lastMeteor;
    private int timesRotated;
    private float Yrotation;
    private float Zrotation;
    private bool increasedSpeed;
    private Renderer boxRend;

    void Start() {
        Vector3 rot = transform.rotation.eulerAngles;
        Yrotation += rot.y;
        timesRotated = 0;
        ladder.SetActive(false);
        boxRend = box.GetComponent<Renderer>();
        
    }

    void Update() {
        DefenceMechanism();
        if (target.GetComponent<Target>().activated)
        {
            Activate();
        }
        Debug.Log(Yrotation);
    }

    public void Activate()
    {
        if (timesRotated == 0)
        {
            boxRend.material = hurt;
            Yrotation += rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, Yrotation, 0);
            if (Yrotation >= 270)
            {
                transform.rotation = Quaternion.Euler(0, 270, 0);
                target.GetComponent<Target>().Deactivate();
                timesRotated++;
            }
        }
        else if (timesRotated == 1)
        {
            Yrotation -= rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, Yrotation, 0);
            if (Yrotation <= 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                target.GetComponent<Target>().Deactivate();
                timesRotated++;
            }
        }
        else if (timesRotated == 2)
        {
            Yrotation += rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, Yrotation, 0);
            if (Yrotation >= 450)
            {
                transform.rotation = Quaternion.Euler(0, 450, 0);
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
                ladder.SetActive(true);
                Destroy(cannon);
                Destroy(gameObject);
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
                    Vector3 spawnPoint = new Vector3(player.position.x, 24.75f, player.position.z);
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
