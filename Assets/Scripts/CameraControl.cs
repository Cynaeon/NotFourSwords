using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	[SerializeField] private float turnSpeed = 4.0f;
	[SerializeField] private float height = 8.0f;
	[SerializeField] private float distance = 7.0f;

	public Transform player;

	private Vector3 offset;

	void Start () {
		offset = new Vector3(player.position.x, player.position.y + height, player.position.z + distance);

	}



	void FixedUpdate()
	{
		offset = Quaternion.AngleAxis (Input.GetAxis ("HorizontalRightStick") * turnSpeed, Vector3.up) * offset;
		//offset = Quaternion.AngleAxis (Input.GetAxis("VerticalRightStick") * turnSpeed, Vector3.right) * offset;

		transform.position = player.position + offset; 
		transform.LookAt (player.position);

	}
		
}