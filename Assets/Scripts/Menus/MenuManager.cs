using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Image teamLogo;
    public Image background;
    public Text infotext;
    public float fadeSpeed;
    public float logoDuration;

    private float currLogoDuration;
    private bool fadingOut;
    private bool showingTeamLogo;
    private bool showingInfotext;
    private bool showingMenu;
    private bool enteringGame;
    private float alpha;

    void Start()
    {
        showingTeamLogo = true;
        currLogoDuration = logoDuration;
    }

    void Update()
    {
        Skip();
        TeamLogo();
        Infotext();
        Menu();

        if (enteringGame)
        {
            alpha += Time.deltaTime * fadeSpeed;
            background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
            if (alpha >= 1)
            {
                SceneManager.LoadScene(1, LoadSceneMode.Single);
            }
        }
    }

    public void PlayButton()
    {
        enteringGame = true;
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private void Skip()
    {
        if (Input.GetButtonDown("P1_Action") || Input.GetButtonDown("P1_Start") || Input.GetButtonDown("P1_Dash") ||
            Input.GetButtonDown("P2_Action") || Input.GetButtonDown("P2_Start") || Input.GetButtonDown("P2_Dash") ||
            Input.GetButtonDown("P3_Action") || Input.GetButtonDown("P3_Start") || Input.GetButtonDown("P3_Dash") ||
            Input.GetButtonDown("P4_Action") || Input.GetButtonDown("P4_Start") || Input.GetButtonDown("P4_Dash"))  {
            showingInfotext = false;
            showingTeamLogo = false;
            showingMenu = true;
        }
    }

    private void Menu()
    {
        if (showingMenu && !enteringGame)
        {
            if (alpha >= 0)
            {
                alpha -= Time.deltaTime * fadeSpeed;
                background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
            }
        }
    }

    private void Infotext()
    {
        if (showingInfotext)
        {
            if (alpha < 1 && !fadingOut)
            {
                alpha += Time.deltaTime * fadeSpeed;
                infotext.color = new Color(infotext.color.r, infotext.color.g, infotext.color.b, alpha);
            }
            else if (alpha >= 1)
            {
                if (currLogoDuration > 0)
                {
                    currLogoDuration -= Time.deltaTime;
                }
                else if (currLogoDuration <= 0)
                {
                    fadingOut = true;
                }
            }
            if (fadingOut)
            {
                alpha -= Time.deltaTime * fadeSpeed;
                infotext.color = new Color(infotext.color.r, infotext.color.g, infotext.color.b, alpha);
                if (alpha <= 0)
                {
                    showingMenu = true;
                    showingInfotext = false;
                    fadingOut = false;
                    alpha = 1;
                }
            }
        }
        else
        {
            infotext.enabled = false;
        }
    }

    private void TeamLogo()
    {
        if (showingTeamLogo)
        {
            if (alpha < 1 && !fadingOut)
            {
                alpha += Time.deltaTime * fadeSpeed;
                teamLogo.color = new Color(teamLogo.color.r, teamLogo.color.g, teamLogo.color.b, alpha);
            }
            else if (alpha >= 1)
            {
                if (currLogoDuration > 0)
                {
                    currLogoDuration -= Time.deltaTime;
                }
                else if (currLogoDuration <= 0)
                {
                    fadingOut = true;
                }
            }
            if (fadingOut)
            {
                alpha -= Time.deltaTime * fadeSpeed;
                teamLogo.color = new Color(teamLogo.color.r, teamLogo.color.g, teamLogo.color.b, alpha);
                if (alpha <= 0)
                {
                    currLogoDuration = logoDuration;
                    showingInfotext = true;
                    showingTeamLogo = false;
                    fadingOut = false;
                    infotext.enabled = true;
                }
            }
        }
        else
        {
            teamLogo.enabled = false;
        }
    }
}

    /*
     * 
    private void Infotext()
    {
        if (!showingMenu && !showingTeamLogo)
        {
            if (showingInfotext)
            {
                if (alpha < 1)
                {
                    alpha += Time.deltaTime * fadeSpeed;
                    infotext.color = new Color(teamLogo.color.r, teamLogo.color.g, teamLogo.color.b, alpha);
                }
                else if (alpha >= 1)
                {
                    if (currLogoDuration > 0)
                    {
                        currLogoDuration -= Time.deltaTime;
                    }
                    else
                    {
                        showingInfotext = false;
                    }
                }
            }
            else
            {
                alpha -= Time.deltaTime * fadeSpeed;
                infotext.color = new Color(teamLogo.color.r, teamLogo.color.g, teamLogo.color.b, alpha);
                if (alpha <= 0)
                {
                    showingMenu = true;
                    alpha = 1;
                }
            }
        }
    }

    private void Menu()
    {
        if (showingMenu)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
        }
    }

    private void TeamLogo()
    {
        if (!showingMenu)
        {
            if (showingTeamLogo)
            {
                if (alpha < 1)
                {
                    alpha += Time.deltaTime * fadeSpeed;
                    teamLogo.color = new Color(teamLogo.color.r, teamLogo.color.g, teamLogo.color.b, alpha);
                }
                else if (alpha >= 1)
                {
                    if (currLogoDuration > 0)
                    {
                        currLogoDuration -= Time.deltaTime;
                    }
                    else
                    {
                        showingTeamLogo = false;
                    }
                }
            }
            else
            {
                alpha -= Time.deltaTime * fadeSpeed;
                teamLogo.color = new Color(teamLogo.color.r, teamLogo.color.g, teamLogo.color.b, alpha);
                if (alpha <= 0)
                {
                    showingInfotext = true;
                    alpha = 1;
                    currLogoDuration = logoDuration;
                }
            }
        }
    }
    */

