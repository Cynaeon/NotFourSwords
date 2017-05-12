using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
{
    public GameObject highlightParticles;

    public void Start()
    {
        highlightParticles.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        highlightParticles.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (highlightParticles)
        {
            highlightParticles.SetActive(false);
        }
    }
}
