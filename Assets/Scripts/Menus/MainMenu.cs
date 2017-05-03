using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    
    public Image fadeOutEffect;
    public float fadeSpeed;
    public Transform menuItems;
    public float transitionSpeed;
    public Text pressStart;
    public Transform[] cats;
    public Transform[] players;

    private bool sceneLoading;
    private int levelToLoad;
    private float alpha;
    private Vector3 mainPos;
    private Vector3 selectPos;
    public enum Transition
    {
        none = 0,
        mainToSelect = 1,
        selectToMain = 2
    }
    [HideInInspector] public Transition transition;
  
    private void Start ()
    {
        mainPos = menuItems.position;
        selectPos = new Vector3(mainPos.x, mainPos.y + 480, mainPos.z);
    }

    private void Update()
    {
        pressStart.enabled = true;
        if (Input.GetButtonDown("P1_Start"))
        {
            LoadScene(1);
        }

        if (sceneLoading)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeOutEffect.color = new Color(fadeOutEffect.color.r, fadeOutEffect.color.g, fadeOutEffect.color.b, alpha);
            
            if (alpha >= 1)
            {
                SceneManager.LoadScene(levelToLoad, LoadSceneMode.Single);
            }
        }

        if (transition == Transition.mainToSelect)
        {
            float step = transitionSpeed * Time.deltaTime;
            menuItems.position = Vector3.MoveTowards(menuItems.position, selectPos, step);
        }

        if (transition == Transition.selectToMain)
        {
            float step = transitionSpeed * Time.deltaTime;
            menuItems.position = Vector3.MoveTowards(menuItems.position, mainPos, step);
        }
    }

	public void LoadScene(int level)
    {
        sceneLoading = true;
        levelToLoad = level;
    }

    public void MainToSelect()
    {
        transition = Transition.mainToSelect;
    }

    public void SelectToMain()
    {
        transition = Transition.selectToMain;
    }
}
