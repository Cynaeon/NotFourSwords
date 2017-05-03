using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class CharacterSelectSpot_old : MonoBehaviour {

    public GameObject blue;
    public GameObject green;
    public GameObject purple;
    public GameObject red;

    public string playerPrefix;

    private bool playerActive;
    private int number;
    private bool axisReset;

	void Start () {
        number = 1;
        
	}
	
	void Update () {
        if (Input.GetButtonDown(playerPrefix + "Action"))
        {
            playerActive = true;
        }
        
        if (playerActive)
        {
            float lookHorizontal = Input.GetAxis(playerPrefix + "HorizontalRightStick");
            Vector3 lookPlayer = new Vector3(0, -lookHorizontal, 0);

            blue.transform.localEulerAngles += lookPlayer * 3;
            green.transform.localEulerAngles += lookPlayer * 3;
            purple.transform.localEulerAngles += lookPlayer * 3;
            red.transform.localEulerAngles += lookPlayer * 3;

            if (Input.GetButtonDown(playerPrefix + "Dash"))
            {
                playerActive = false;
            }

            if (number == 1)
            {
                green.SetActive(false);
                red.SetActive(false);
                blue.SetActive(true);
            }
            if (number == 2)
            {
                purple.SetActive(false);
                blue.SetActive(false);
                green.SetActive(true);
            }
            if (number == 3)
            {
                red.SetActive(false);
                green.SetActive(false);
                purple.SetActive(true);
            }
            if (number == 4)
            {
                blue.SetActive(false);
                purple.SetActive(false);
                red.SetActive(true);
            }

            if (!axisReset && Input.GetAxis(playerPrefix + "Horizontal") > 0.4)
            {
                number++;
                axisReset = true;
            }

            if (!axisReset && Input.GetAxis(playerPrefix + "Horizontal") < -0.4)
            {
                number--;
                axisReset = true;
            }

            if (number >= 5)
            {
                number = 1;
            }
            if (number <= 0)
            {
                number = 4;
            }

            if (Input.GetAxis(playerPrefix + "Horizontal") == 0)
            {
                axisReset = false;
            }
            
            if (Input.GetButtonDown(playerPrefix + "Start"))
            {
                GameObject character = GetPlayerCharacter();
                SceneManager.LoadScene("outside_tower", LoadSceneMode.Single);
            }
        }
        else
        {
            blue.SetActive(false);
            green.SetActive(false);
            purple.SetActive(false);
            red.SetActive(false);
        }
    }

    private GameObject GetPlayerCharacter()
    {
        if (number == 1)
        {
            return blue;
        }
        if (number == 2)
        {
            return green;
        }
        if (number == 3)
        {
            return purple;
        }
        if (number == 4)
        {
            return red;
        }
        return null;
    }
}
