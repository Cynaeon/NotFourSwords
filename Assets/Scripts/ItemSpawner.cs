using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {
    List<GameObject> Items = new List<GameObject>();

    public GameObject none;
    public GameObject jumpItem;
    public GameObject seeThroughItem;
    public GameObject magnetItem;
    
    [SerializeField] private int _active = 1;


    // Use this for initialization
    void Start()
    {
        Items.Add(none);
        Items.Add(jumpItem);
        Items.Add(seeThroughItem);
        Items.Add(magnetItem);
        Items[_active].SetActive(true);
	}

    public int checkActive()
    {
        return _active;
    }

    public void changeActive(int receivedItem)
    {
            Items[_active].SetActive(false);
            _active = receivedItem;
            Items[_active].SetActive(true);
    }
}
