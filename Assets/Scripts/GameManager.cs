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

	// Use this for initialization
	void Start () {
        levelExits = GameObject.FindGameObjectsWithTag("Start");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
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
