using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject[] players;
    public GameObject[] cameras;
    public GameObject[] levelExits;
    public int lastDoorID;
    public Transform startPos;

    public bool disableMovement;
    public bool[] doorOpened;
    public ItemSpawner._items[] itemOnSpawner;

	void Start () {
        levelExits = GameObject.FindGameObjectsWithTag("Start");
        doorOpened = new bool[10];

        SetSpawnerItems();
	}

    // Set all the itemspawner items here!! NOT in the inspector!
    private void SetSpawnerItems()
    {
        itemOnSpawner = new ItemSpawner._items[10];

        // Hide and Seek
        itemOnSpawner[0] = ItemSpawner._items.key;
        // Corridor (Jump)
        itemOnSpawner[1] = ItemSpawner._items.boots;
        itemOnSpawner[2] = ItemSpawner._items.boots;
        itemOnSpawner[3] = ItemSpawner._items.boots;
        itemOnSpawner[4] = ItemSpawner._items.boots;
        // Labyrinth
        itemOnSpawner[5] = ItemSpawner._items.monocle;
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
