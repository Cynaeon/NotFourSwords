using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Image seeThroughImage;
    public Image jumpImage;
    public Image magnetImage;
    public Image swordImage;
    public Image notificationImage;
    public Image crosshair;
    public Text health;
    public Text coins;
    public Text paused;
	public Text pausedShadow;
    public GameObject player;
    private float maxHealth;
    private float currentHealth;
    private float Coins;

	void Start () {
        seeThroughImage.enabled = false;
        jumpImage.enabled = false;
        magnetImage.enabled = false;
        notificationImage.enabled = false;
        swordImage.enabled = false;
		paused.enabled = false;
		pausedShadow.enabled = false;
        crosshair.enabled = false;

        maxHealth = player.GetComponent<PlayerControl>().maxHealth;
        currentHealth = player.GetComponent<PlayerControl>().currentHealth;
        Coins = player.GetComponent<PlayerControl>().gatheredCoins;
        health.text = "HP: " + currentHealth + " / " + maxHealth;
        coins.text = "Coins: " + coins;
	}
	
	// Update is called once per frame
	void Update () {
        maxHealth = player.GetComponent<PlayerControl>().maxHealth;
        currentHealth = player.GetComponent<PlayerControl>().currentHealth;
        health.text = "HP: " + currentHealth + " / " + maxHealth;
        Coins = player.GetComponent<PlayerControl>().gatheredCoins;
        coins.text = "Coins: " + Coins;

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

    public void UIItems(bool jump, bool lens, bool magnet, bool sword)
    {
        jumpImage.enabled = jump;
        seeThroughImage.enabled = lens;
        magnetImage.enabled = magnet;
        swordImage.enabled = sword;
    }

	public void UIPause(bool isPaused) {
		paused.enabled = isPaused;
		pausedShadow.enabled = isPaused;
	}
}
