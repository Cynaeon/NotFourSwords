using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject[] players;
    public GameObject[] cameras;
    public GameObject[] levelExits;
    public int lastDoorID;
    public int combinedScore;
    public int[] UpgradeLevel;
    private int upgraded;
    public Transform startPos;

    public bool disableMovement;
    public bool keyPicked_1;
    public bool doorOpened_1;

	void Start () {
        levelExits = GameObject.FindGameObjectsWithTag("Start");
	}

    public void DisableMovement()
    {
        disableMovement = true;
    }

    public void EnableMovement()
    {
        disableMovement = false;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Entrance")
        {
            Transform start = GameObject.FindGameObjectWithTag("Start").transform;
            startPos = start.transform;
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerControl>().SetStartPosition(startPos);
            }
            foreach (GameObject camera in cameras)
            {
                camera.GetComponent<CameraControl>().SetStartPosition();
            }

        }
        if (scene.name != "Entrance")
        {
            levelExits = GameObject.FindGameObjectsWithTag("Start");

            foreach (GameObject exit in levelExits)
            {
                if (exit.GetComponent<LevelExit>().doorID == lastDoorID)
                {
                    startPos = exit.GetComponent<LevelExit>().startPosition;
                    lastDoorID = exit.GetComponent<LevelExit>().doorID;
                }
            }

            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerControl>().SetStartPosition(startPos);
            }
            foreach (GameObject camera in cameras)
            {
                camera.GetComponent<CameraControl>().SetStartPosition();
            }
        }
    }
    
    public void TotalScore(int value)
    {
        combinedScore = combinedScore + value;
        if(combinedScore >= UpgradeLevel[upgraded])
        {
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerControl>().IncreaseShootingLevel(1);
            }
            if(upgraded < UpgradeLevel.Length-1) {
                upgraded++;
            }
        }
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. 
        //Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }



}
