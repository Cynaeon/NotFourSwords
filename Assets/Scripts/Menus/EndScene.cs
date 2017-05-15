using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour {

    public Image gameLogo;
    public Text endText;
    public float fadeSpeed;

    private float logoAlpha;
    private float textAlpha;

	void Start () {
        logoAlpha = 0;
        textAlpha = 0;
        gameLogo.color = new Color(gameLogo.color.r, gameLogo.color.g, gameLogo.color.b, 0);
        endText.color = new Color(endText.color.r, endText.color.g, endText.color.b, 0);
	}

    void Update()
    {
        if (logoAlpha < 1)
        {
            logoAlpha += Time.deltaTime * fadeSpeed;
            
        }
        else
        {
            textAlpha += Time.deltaTime * fadeSpeed;
        }
        gameLogo.color = new Color(gameLogo.color.r, gameLogo.color.g, gameLogo.color.b, logoAlpha);
        endText.color = new Color(endText.color.r, endText.color.g, endText.color.b, textAlpha);

        if (textAlpha >= 1)
        {
            if (Input.GetButtonDown("P1_Action") || Input.GetButtonDown("P1_Start") || Input.GetButtonDown("P1_Dash") ||
            Input.GetButtonDown("P2_Action") || Input.GetButtonDown("P2_Start") || Input.GetButtonDown("P2_Dash") ||
            Input.GetButtonDown("P3_Action") || Input.GetButtonDown("P3_Start") || Input.GetButtonDown("P3_Dash") ||
            Input.GetButtonDown("P4_Action") || Input.GetButtonDown("P4_Start") || Input.GetButtonDown("P4_Dash"))
            {
                Application.Quit();

            }
        }
    }
}
