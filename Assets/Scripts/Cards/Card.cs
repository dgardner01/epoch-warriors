using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler
{
    CardData data => GetComponent<CardData>();

    [Header("Display")]
    public TextMeshProUGUI title;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI description;
    public Image type;
    public Sprite attack, block, grab;
    public GameObject[] comboLinks;

    Vector3 startPos;
    Vector3 endPos;
    Vector3 currentPos;

    [Header("Interaction")]
    public float hoverSpeed;
    public float hoverOffset;
    public float playHeight;
    public bool enemy;

    bool activePlay;
    bool hovering;
    bool dragging;
    bool placed;
    public bool playable = true;

    CardManager cardManager => FindAnyObjectByType<CardManager>();
    GameManager gameManager => FindAnyObjectByType<GameManager>();

    Animator animator => GetComponent<Animator>();

    void Start()
    {
        startPos = Vector3.zero;
        endPos = startPos + Vector3.up * hoverOffset;
        InitializeCard();
        if (enemy)
        {
            animator.SetTrigger("enter");
        }
    }
    void InitializeCard()
    {
        title.text = data.title;
        cost.text = "" + data.cost;
        switch (data.cardType)
        {
            case CardData.Type.Attack:
                type.sprite = attack;
                break;
            case CardData.Type.Block:
                type.sprite = block;
                break;
            case CardData.Type.Grab:
                type.sprite = grab;
                break;
        }
        for (int i = 0; i < comboLinks.Length; i++)
        {
            if (i == data.order)
            {
                comboLinks[i].SetActive(true);
            }
            else
            {
                comboLinks[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if the game is in Card Play state & playable, play is active
        activePlay = gameManager.gameState == GameManager.GameState.CardPlay && playable && !enemy;
        if (activePlay)
        {
            animator.enabled = false;
            if (dragging)
            {
                DragCard();
            }
        }
        else
        {
            animator.enabled = true;
        }
    }
    private void FixedUpdate()
    {
        if (activePlay && !dragging && !placed)
        {
            FloatOnHover();
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
