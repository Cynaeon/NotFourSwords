using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour {

    public int ManaValue;

    void Start()
    {
        if(ManaValue == 0)
        {
            Debug.LogError("Mana value not set");
        }
    }
}
