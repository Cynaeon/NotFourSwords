using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    List<GameObject> Items = new List<GameObject>();
    public GameObject none;
    public GameObject boots;
    public GameObject monocle;
    public GameObject magnet;
    public GameObject sword;
    public GameObject key;

    public enum _items
    {
        none,
        boots,
        monocle,
        magnet,
        sword,
        key
    }

    public _items _active;
    public int spawnerID;
    public bool debugMode;

    private GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        Items.Add(none);
        Items.Add(boots);
        Items.Add(monocle);
        Items.Add(magnet);
        Items.Add(sword);
        Items.Add(key);
        if (!debugMode)
        {
            _active = gameManager.GetComponent<GameManager>().itemOnSpawner[spawnerID];
        }
        Items[(int)_active].SetActive(true);
    }

    public int checkActive()
    {
        return (int)_active;
    }

    public void changeActive(int receivedItem)
    {
        Items[(int)_active].SetActive(false);
        switch (receivedItem)
        {
            case 0:
                _active = _items.none;
                break;
            case 1:
                _active = _items.boots;
                break;
            case 2:
                _active = _items.monocle;
                break;
            case 3:
                _active = _items.magnet;
                break;
            case 4:
                _active = _items.sword;
                break;
            case 5:
                _active = _items.key;
                break;
        }
        if (!debugMode)
        {
            gameManager.GetComponent<GameManager>().itemOnSpawner[spawnerID] = _active;
        }
        Items[(int)_active].SetActive(true);
    }
}
