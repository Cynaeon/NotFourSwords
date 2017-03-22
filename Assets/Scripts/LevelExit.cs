using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelExit : MonoBehaviour {

    public GameObject[] screenFadeOut;
    public float fadeSpeed;
    public GameObject[] players;
    public Transform elevator;

    public enum Level
    {
        CharacterSelect = 0,
        OutsideTowerDemo = 1,
        Entrance = 2,
        Labyrinth = 3,
        Deflect = 4,
        MagneticBlock = 5,
        Enemies = 6,
        RotatingCube = 7
    }

    public Level level;

    private Color screenFade;
    private float screenAlpha;
    private bool sceneEntering;
    private bool sceneExiting;

	void Awake () {
        sceneEntering = true;
        screenAlpha = 1;
        screenFadeOut = GameObject.FindGameObjectsWithTag("ScreenFadeOut");
        foreach (GameObject image in screenFadeOut)
        {
            image.GetComponent<Image>().color = new Color(screenFade.r, screenFade.g, screenFade.b, screenAlpha);
        }
        
    }

	void Update () {
        if (sceneEntering)
        {
            screenAlpha -= Time.deltaTime * fadeSpeed;
            foreach (GameObject image in screenFadeOut)
            {
                image.GetComponent<Image>().color = new Color(screenFade.r, screenFade.g, screenFade.b, screenAlpha);
            }
            if (screenAlpha <= 0)
            {
                sceneEntering = false;
            }
        }

		if (sceneExiting)
        {
            if (elevator)
            {
                elevator.position += Vector3.up * Time.deltaTime * 5;
            }

            screenAlpha += Time.deltaTime * fadeSpeed;
            foreach (GameObject image in screenFadeOut)
            {
                image.GetComponent<Image>().color = new Color(screenFade.r, screenFade.g, screenFade.b, screenAlpha);
            }
            if (screenAlpha >= 1)
            {
                SceneManager.LoadScene((int)level, LoadSceneMode.Single);
                sceneEntering = true;
                foreach (GameObject player in players)
                {
                    player.GetComponent<PlayerControl>().settingStartPos = true;
                }
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            sceneExiting = true;
        }
    }
}
