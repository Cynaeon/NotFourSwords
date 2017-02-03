using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	[SerializeField] private float turnSpeed = 4.0f;
	[SerializeField] private float height = 8.0f;
	[SerializeField] private float distance = 7.0f;

	public Transform player;
	public Transform lockOnCameraSpot;
    public string playerPrefix;

	private Vector3 offset;


	void Start () {
		offset = new Vector3(player.position.x, player.position.y + height, player.position.z + distance);

	}

	void Update() {
	
	}

	void LateUpdate()
	{
		
		if (Input.GetButton (playerPrefix + "LockOn")) {
			transform.position = Vector3.MoveTowards(transform.position, lockOnCameraSpot.position, turnSpeed / 2);
			offset = Quaternion.AngleAxis (0, Vector3.up) * offset;
		} else {
			//offset += Quaternion.AngleAxis (Input.GetAxis (playerPrefix + "HorizontalRightStick") * turnSpeed, Vector3.up);
			//offset = Quaternion.AngleAxis (Input.GetAxis("VerticalRightStick") * turnSpeed, Vector3.right) * offset;
			transform.position = Vector3.MoveTowards(transform.position, player.position + offset, turnSpeed / 2); 
		}
		transform.LookAt (player.position);
		//Debug.Log (offset);   
	}
		
}