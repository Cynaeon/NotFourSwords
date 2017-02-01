using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour {

	public Transform start;
	public Transform end;
	public float speed = 5.0f;

	private float startTime;
	private float journeyLength;
	void Start() {
		startTime = Time.time;
		journeyLength = Vector3.Distance(start.position, end.position);
	}
	void Update() {
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(start.position, end.position, fracJourney);
		if (fracJourney >= 1) {
			Transform temp = end;
			end = start;
			start = temp;
			startTime = Time.time;
		}
	}
}
