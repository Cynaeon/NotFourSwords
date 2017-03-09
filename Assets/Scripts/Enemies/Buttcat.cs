using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttcat : Enemy {

    public float aggroRange;
    public float speed;

	public override void Start () {
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
}
