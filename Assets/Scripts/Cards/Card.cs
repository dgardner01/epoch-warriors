using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    Vector3 startPos;
    Vector3 endPos;
    Vector3 currentPos;

    public float hoverSpeed;
    public float hoverOffset;
    public float playHeight;

    bool activePlay;
    bool hovering;
    bool dragging;
    bool placed;
    public bool playable = true;

    CardManager cardManager => FindAnyObjectByType<CardManager>();
    GameManager gameManager => FindAnyObjectByType<GameManager>();

    void Start()
    {
        startPos = transform.localPosition;
        endPos = startPos + Vector3.up * hoverOffset;
    }

    // Update is called once per frame
    void Update()
    {
        //if the game is in Card Play state & playable, play is active
        activePlay = gameManager.gameState == GameManager.GameState.CardPlay && playable;
        if (activePlay)
        {
            if (!dragging)
            {
                FloatOnHover();
            }
            else
            {
                DragCard();
            }
        }
    }

    public void FloatOnHover()
    {
        //if hovering, move card y to offset
        //if not hovering, move back to start y
        //lerp for smooth animation

        currentPos = Vector3.Lerp(currentPos,hovering ? endPos : startPos, hoverSpeed);
        currentPos.z = 0;

        transform.localPosition = currentPos;

        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public void DragCard()
    {
        Vector3 cardPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cardPos.z = 0;

        transform.position = cardPos;

        //reset rotation
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void OnPointerEnter(PointerEventData pointerEnter)
    {
        hovering = true;
        if (!placed && activePlay)
        {
            //make card render in front of all other cards
            transform.parent.SetAsLastSibling();
        }
    }

    public void OnPointerExit(PointerEventData pointerExit)
    {
        hovering = false;
    }

    public void OnPointerDown(PointerEventData pointerDown)
    {
        dragging = true;
    }

    public void OnPointerUp(PointerEventData pointerUp)
    {
        dragging = false;
        if (activePlay)
        {
            if (transform.localPosition.y > playHeight)
            {
                cardManager.PlaceCard(gameObject, cardManager.playArea);
                placed = true;
            }
            else
            {
                cardManager.PlaceCard(gameObject, cardManager.hand);
                placed = false;
            }
        }
    }
}
