using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    List<GameObject> Items = new List<GameObject>();
    public GameObject none;
    public GameObject feather;
    public GameObject lens;
    public GameObject magnet;
    public GameObject sword;

    public enum _items
    {
        none,
        feather,
        lens,
        magnet,
        sword
    }

    public _items _active;


    // Use this for initialization
    void Start()
    {
        Items.Add(none);
        Items.Add(feather);
        Items.Add(lens);
        Items.Add(magnet);
        Items.Add(sword);
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
                _active = _items.feather;
                break;
            case 2:
                _active = _items.lens;
                break;
            case 3:
                _active = _items.magnet;
                break;
            case 4:
                _active = _items.sword;
                break;
        }

        Items[(int)_active].SetActive(true);
    }
}
