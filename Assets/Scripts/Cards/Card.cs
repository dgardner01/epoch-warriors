using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    float start_y = 0;
    float end_y = 0;
    float current_y = 0;
    public float hoverSpeed;
    public float hoverOffset;
    bool hovering = false;
    // Start is called before the first frame update
    void Start()
    {
        start_y = transform.localPosition.y;
        end_y = start_y + hoverOffset;
    }

    // Update is called once per frame
    void Update()
    {
        FloatOnHover();
    }

    public void FloatOnHover()
    {
        current_y = Mathf.Lerp(current_y,hovering ? end_y : start_y, hoverSpeed);

        transform.localPosition = 
        new Vector3(transform.localPosition.x,current_y,transform.localPosition.z);
    }

    public void OnPointerEnter(PointerEventData pointerEnter)
    {
        hovering = true;
        transform.parent.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData pointerExit)
    {
        hovering = false;
    }
}
