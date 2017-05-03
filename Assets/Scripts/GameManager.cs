using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject[] players;
    public GameObject[] cameras;
    public GameObject[] levelExits;
    public GameObject cameraManager;
    public GameObject playerManager;
    public int lastDoorID;
    public int combinedScore;
    public int[] UpgradeLevel;
    private int upgraded;
    public Transform startPos;

    public bool disableMovement;
    public bool[] doorOpened;
    public ItemSpawner._items[] itemOnSpawner;

    public bool[] floorsUnlocked = new bool[9];
    public int attempts;
    public int currentFloor;
    public Canvas[] canvases;

	void Start () {
        levelExits = GameObject.FindGameObjectsWithTag("Start");
        doorOpened = new bool[50];
        /*floorsUnlocked[1] = true;
        floorsUnlocked[2] = true;
        floorsUnlocked[3] = true;
        floorsUnlocked[4] = true;
        floorsUnlocked[5] = true;
        floorsUnlocked[6] = true;
        floorsUnlocked[7] = true;
        floorsUnlocked[8] = true;*/
        currentFloor = 1;
        SetSpawnerItems();
	}

    // Set all the itemspawner items here!! NOT in the inspector!
    private void SetSpawnerItems()
    {
        itemOnSpawner = new ItemSpawner._items[10];

        // Hide and Seek
        itemOnSpawner[0] = ItemSpawner._items.key;
        itemOnSpawner[1] = ItemSpawner._items.key;
        // Corridor (Jump)
        itemOnSpawner[2] = ItemSpawner._items.boots;
        itemOnSpawner[3] = ItemSpawner._items.boots;
        itemOnSpawner[4] = ItemSpawner._items.boots;
        itemOnSpawner[5] = ItemSpawner._items.boots;
        // Labyrinth
        itemOnSpawner[6] = ItemSpawner._items.monocle;
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
        if (scene.name == "Menu")
        {
            disableMovement = true;
            cameraManager.SetActive(false);
            //playerManager.SetActive(false);
        }
        else if (scene.name == "Entrance")
        {
            disableMovement = false;
            cameraManager.SetActive(true);
            playerManager.SetActive(true);
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
        else if (scene.name != "Entrance")
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
            foreach(GameObject player in players)
            {
                player.GetComponent<PlayerControl>().IncreaseShootingLevel(1);
            }
            if(upgraded < UpgradeLevel.Length - 1)
            {
                upgraded++;
            }
        }
    }

    public void SetCharacterToPlayer()
    {

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
