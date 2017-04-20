using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHeart : MonoBehaviour {

    public Sprite[] pieces;
    public Image image;
    

    public void ChangeHealth(int currentHealth)
    {
        gameObject.GetComponent<Image>().sprite = pieces[currentHealth];
    }

}
