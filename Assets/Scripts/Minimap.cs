using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Minimap : MonoBehaviour {

    public Camera minimapCamera;
    public string playerPrefix;
    public GameObject minimapIcon;
    public Transform player;
    public Color highlightColor;
    public Camera playerCamera;
    public Image screenDimming;

    private SpriteRenderer _rend;
    private Color startColor;

	void Start () {
        minimapCamera.enabled = false;
        _rend = minimapIcon.GetComponent<SpriteRenderer>();
        startColor = _rend.color;
	}

	void Update () {
        minimapCamera.rect = playerCamera.rect;
        minimapIcon.transform.position = new Vector3(player.position.x, 100, player.position.z);

        _rend.color = Color.Lerp(startColor, highlightColor, Mathf.PingPong(Time.time, 0.5f));

        if (Input.GetButton(playerPrefix +  "Back") && player.gameObject.activeSelf)
        {
            screenDimming.enabled = true;
            minimapCamera.enabled = true;
        } else
        {
            screenDimming.enabled = false;
            minimapCamera.enabled = false;
        }
	}
        
}
