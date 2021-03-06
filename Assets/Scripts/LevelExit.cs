﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelExit : MonoBehaviour
{

    public GameObject[] screenFadeOut;
    public float fadeSpeed;
    public GameObject[] players;
    public int doorID;
    public Transform startPosition;

    public enum Level
    {
        Entrance = 0,
        OutsideTowerDemo = 1,
        CharacterSelect = 2,
        Labyrinth = 3,
        Deflect = 4,
        MagneticBlock = 5,
        Enemies = 6,
        RotatingCube = 7,
        Elevator = 8,
        Floor1WestCorridor = 9,
        Hide_and_Seek = 10,
        Floor1EastCorridor = 11,
        JumpPuzzle = 12,
        Floor2NorthCorridor = 13,
        Floor2SouthCorridor = 14,
        Corridor_Empty_3 = 15, //Placeholder
        Corridor_Empty_4 = 16, //Placeholder
        DarkPuzzle = 17,
        Corridor_Torch = 18,
        Floor3EastCorridor = 19,
        LensPuzzle = 20,
        Floor4WestCorridor = 21,
        Floor4EastCorridor = 22,
        DemoEnding = 23,
        Floor3WestCorridor
    }

    public Level level;

    private GameObject[] playersInGame;
    private List<Collider> playersAtExit = new List<Collider>();
    private GameObject gameManager;
    private Color screenFade;
    private float screenAlpha;
    private bool sceneEntering;
    private bool sceneExiting;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager");
        sceneEntering = true;
        screenAlpha = 1;
        screenFadeOut = GameObject.FindGameObjectsWithTag("ScreenFadeOut");
        foreach (GameObject image in screenFadeOut)
        {
            image.GetComponent<Image>().color = new Color(screenFade.r, screenFade.g, screenFade.b, screenAlpha);
        }
    }

    void Update()
    {
        playersInGame = GameObject.FindGameObjectsWithTag("Player");
        if (playersInGame.Length > 0 && playersAtExit.Count == playersInGame.Length)
        {
            GameObject gameManager = GameObject.Find("GameManager");
            gameManager.GetComponent<GameManager>().lastDoorID = doorID;
            sceneExiting = true;
        }

        if (sceneEntering)
        {
            gameManager.GetComponent<GameManager>().DisableMovement();
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

        else if (sceneExiting)
        {
            gameManager.GetComponent<GameManager>().DisableMovement();
            screenAlpha += Time.deltaTime * fadeSpeed;
            foreach (GameObject image in screenFadeOut)
            {
                image.GetComponent<Image>().color = new Color(screenFade.r, screenFade.g, screenFade.b, screenAlpha);
            }
            if (screenAlpha >= 1)
            {
                SceneManager.LoadScene((int)level, LoadSceneMode.Single);
                sceneEntering = true;
            }
        }
        else
        {
            gameManager.GetComponent<GameManager>().EnableMovement();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!playersAtExit.Contains(other))
            {
                playersAtExit.Add(other);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (playersAtExit.Contains(other))
            {
                playersAtExit.Remove(other);
            }
        }
    }
}
