using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUITest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    CardUIManagerTest manager => FindAnyObjectByType<CardUIManagerTest>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        manager.hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manager.hovering = false;
    }
}
