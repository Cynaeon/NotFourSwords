using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour {

    public Camera minimapCamera;
    public string playerPrefix;
    public GameObject minimapIcon;
    public Transform player;
    public Color highlightColor;
    public Camera playerCamera;

    private SpriteRenderer _rend;
    private Color startColor;


	// Use this for initialization
	void Start () {
        minimapCamera.enabled = false;
        _rend = minimapIcon.GetComponent<SpriteRenderer>();
        startColor = _rend.color;
	}
	
	// Update is called once per frame
	void Update () {

        minimapCamera.rect = playerCamera.rect;
        minimapIcon.transform.position = new Vector3(player.position.x, 100, player.position.z);

        _rend.color = Color.Lerp(startColor, highlightColor, Mathf.PingPong(Time.time, 0.5f));

        if (Input.GetButton(playerPrefix +  "Back"))
        {
            minimapCamera.enabled = true;
        } else
        {
            minimapCamera.enabled = false;
        }
	}
        
}
