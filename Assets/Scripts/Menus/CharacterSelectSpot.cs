using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectSpot : MonoBehaviour {

    public string playerPrefix;
    public RawImage character;
    public GameObject[] arrows;
    public bool inSelectScreen;
    public GameObject mainCanvas;
    public Text joinText;

    private bool joined;
    private bool axisReset;
    private MainMenu mainMenu;

	void Start () {
        SetActiveCharacter();
        DisableArrows();
        mainMenu = mainCanvas.GetComponent<MainMenu>();
	}

	void Update () {
        if (inSelectScreen)
        {
            PlayerInput();
        }

        if (mainMenu.transition == MainMenu.Transition.mainToSelect)
        {
            inSelectScreen = true;
        } 
        else
        {
            inSelectScreen = false;
        }
	}

    private void PlayerInput()
    {
        
        if (Input.GetButtonDown(playerPrefix + "Action"))
        {
            joined = true;
        }

        if (!joined)
        {
            /*
            if (Input.GetAxis(playerPrefix + "Horizontal") > 0.5f && !axisReset)
            {
                axisReset = true;
                NextCharacter();
            }
            else if (Input.GetAxis(playerPrefix + "Horizontal") < -0.5f && !axisReset)
            {
                axisReset = true;
                PreviousCharacter();
            }

            else if (Input.GetAxis(playerPrefix + "Horizontal") == 0)
            {
                axisReset = false;
            }
            */
            if (Input.GetButtonDown(playerPrefix + "Dash"))
            {
                mainMenu.SelectToMain();
            }
        }
        else
        {
            if (Input.GetButtonDown(playerPrefix + "Dash"))
            {
                joined = false;
            }
        }

        if (joined)
        {
            character.enabled = true;
            joinText.enabled = false;
        }
        else
        {
            character.enabled = false;
            joinText.enabled = true;
        }
        /*
        if (lockedCharacter)
        {
            DisableArrows();
        }
        else
        {
            EnableArrows();
        }
        */
    }

    /*
    public void NextCharacter()
    {
        selectedCharacter++;
        if (selectedCharacter == 4)
        {
            selectedCharacter = 0;
        }
        SetActiveCharacter();
    }
    

    public void PreviousCharacter()
    {
        selectedCharacter--;
        if (selectedCharacter == -1)
        {
            selectedCharacter = 3;
        }
        SetActiveCharacter();
    }
    */

    public void SetActiveCharacter()
    {
        character.enabled = true;
    }
    /*
    private void SetDefaultCharacter()
    {
        if (playerPrefix == "P1_")
        {
            selectedCharacter = 0;
        }
        else if (playerPrefix == "P2_")
        {
            selectedCharacter = 1;
        }
        else if (playerPrefix == "P3_")
        {
            selectedCharacter = 2;
        }
        else if (playerPrefix == "P4_")
        {
            selectedCharacter = 3;
        }
    }
    */
    private void DisableArrows()
    {
        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(false);
        }
    }

    private void EnableArrows()
    {
        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(true);
        }
    }
}
