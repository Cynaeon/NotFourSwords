using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Image seeThroughImage;
    public Image jumpImage;
    public Image magnetImage;
    public Image swordImage;
    public Image keyImage;
    public Image notificationImage;
    public Image ItemNotification;
    public Image crosshair;
    public Image hearts;
    public Text score;
    public Text paused;
	public Text pausedShadow;
    public GameObject player;
    private int currentHealth;
    private float Score;

	void Start () {
        seeThroughImage.enabled = false;
        jumpImage.enabled = false;
        magnetImage.enabled = false;
        notificationImage.enabled = false;
        swordImage.enabled = false;
		paused.enabled = false;
		pausedShadow.enabled = false;
        crosshair.enabled = false;
        keyImage.enabled = false;
        
        currentHealth = player.GetComponent<PlayerControl>().currentHealth;
        Score = player.GetComponent<PlayerControl>().gatheredScore;
        
        score.text = "Mana: " + score;
	}
	
	// Update is called once per frame
	void Update () {
        
        Score = player.GetComponent<PlayerControl>().gatheredScore;
        score.text = "Mana: " + Score;
        
        if (player.GetComponent<PlayerControl>().firstPerson)
        {
            crosshair.enabled = true;
        }
        else
        {
            crosshair.enabled = false;
        }
    }

    public void EnableNotification(bool state)
    {
        notificationImage.enabled = state;
    }

    public void EnableItemNotification(bool state)
    {
        ItemNotification.enabled = state;
    }

    public void UpdateHealth(int currentHealth)
    {
        hearts.gameObject.GetComponent<UIHeart>().ChangeHealth(currentHealth);
    }

    public void UIItems(bool jump, bool lens, bool magnet, bool sword, bool key)
    {
        jumpImage.enabled = jump;
        seeThroughImage.enabled = lens;
        magnetImage.enabled = magnet;
        swordImage.enabled = sword;
        keyImage.enabled = key;
    }

	public void UIPause(bool isPaused) {
		paused.enabled = isPaused;
		pausedShadow.enabled = isPaused;
	}
}
