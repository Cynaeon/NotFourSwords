using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMana : MonoBehaviour {

    private Image Mana;
    // Use this for initialization
    void Start () {
        Mana = gameObject.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        FillMana();
	}

    public void FillMana()
    {
        Mana.fillAmount += 0.1f;
    }
}
