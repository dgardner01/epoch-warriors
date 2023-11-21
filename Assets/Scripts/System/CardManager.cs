using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public GameObject[] deck;

    public List<GameObject> deckCards;
    public List<GameObject> handCards;

    public GameObject playArea;
    public GameObject arrows;
    public GameObject hand;

    UIManager UI => FindAnyObjectByType<UIManager>();
    GameManager gameManager => FindAnyObjectByType<GameManager>();
    // Start is called before the first frame update
    void Start()
    {
        InitializeDeckList();
        DrawCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitializeDeckList()
    {
        for (int i = 0; i < deck.Length; i++)
        {
            deckCards.Add(deck[i]);
        }
    }
    public void DrawCards()
    {
        print("hi");
        Transform _hand = hand.transform;
        for (int i = 0; i < _hand.childCount; i++)
        {
            if (_hand.GetChild(i).childCount == 0)
            {
                GameObject drawnCard = deck[Random.Range(0, deck.Length)];
                deckCards.Remove(drawnCard);
                GameObject cardInstance = Instantiate(drawnCard, 
                    _hand.GetChild(i).transform.position, Quaternion.identity);
                PlaceCard(cardInstance, hand);
            }
            else
            {
                _hand.GetChild(i).GetChild(0).GetComponent<Card>().playable = true;
            }
        }
        UpdatePlayableHand();
    }
    public void PlaceCard(GameObject card, GameObject container)
    {
        for (int i = 0; i < container.transform.childCount; i++)
        {
            if (container.transform.GetChild(i).childCount == 0)
            {
                //make card a child of first empty slot in play area
                Transform cardParent = container.transform.GetChild(i);
                card.transform.SetParent(cardParent);
                card.transform.localScale = Vector3.one;
                card.transform.localPosition = Vector3.zero;
                card.transform.localRotation = Quaternion.Euler(Vector3.zero);
                UpdatePlayArea();
                return;
            }
        }
    }
    public void UpdatePlayArea()
    {
        //move parent position to center cards
        float _x = Mathf.Lerp(465, 0, CardsInPlayArea() / playArea.transform.childCount);
        Vector3 parentPos = playArea.transform.localPosition;
        parentPos.x = _x;
        playArea.transform.localPosition = parentPos;
        arrows.transform.localPosition = parentPos;

        //draw combo arrows
        for (int i = 0; i < arrows.transform.childCount; i++)
        {
            Transform _arrows = arrows.transform;
            if (CardsInPlayArea() > 1 && i < CardsInPlayArea()-1)
            {
                _arrows.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                _arrows.GetChild(i).gameObject.SetActive(false);
            }
        }

        //sort play area
        for (int i = 0; i < playArea.transform.childCount; i++)
        {
            //is the last slot in index
            Transform _playArena = playArea.transform;
            if ((i - 1) > -1 && _playArena.GetChild(i).childCount == 1)
            {
                //if last slot is empty move to that slot
                if (_playArena.GetChild(i - 1).childCount == 0)
                {
                    _playArena.GetChild(i).GetChild(0).SetParent(_playArena.GetChild(i - 1));
                }
            }
        }

        //show unplayable cards
        if (gameManager.gameState == GameManager.GameState.CardPlay)
        {
            UpdatePlayableHand();
        }

        //update spirit meter
        UI.UpdateSpiritMeter(TotalSpiritInPlayArea());

        
    }

    public void UpdatePlayableHand()
    {
        Transform _hand = hand.transform;

        //see what combo order the first and last card is
        int _firstOrder = 0;
        int _lastOrder = 0;
        if (playArea.transform.GetChild(0).childCount > 0)
        {
            _firstOrder = playArea.transform.GetChild(0).GetChild(0).GetComponent<CardData>().order;
        }
        int _last = (int)(CardsInPlayArea()-1);
        if (_last > -1 && playArea.transform.GetChild(_last).childCount > 0)
        {
            _lastOrder = playArea.transform.GetChild(_last).GetChild(0).GetComponent<CardData>().order;
        }

        //cards are unplayable if not started with a starter or an ender is in play
        for (int i = 0; i < _hand.childCount; i++)
        {
            if (_hand.GetChild(i).childCount > 0)
            {
                GameObject _cardObj = _hand.GetChild(i).GetChild(0).gameObject;
                CardData _cardData = _cardObj.GetComponent<CardData>();
                Card _card = _cardObj.GetComponent<Card>();
                if (_cardData.order < 1 && CardsInPlayArea() > 0 || _firstOrder > 0 || _lastOrder == 2)
                {
                    _cardObj.GetComponent<Image>().color = Color.gray;
                    _card.playable = false;
                }
                else
                {
                    _cardObj.GetComponent<Image>().color = Color.white;
                    _card.playable = true;
                }
            }
        }

        //cannot play a combo unless it is single or has a starter and ender
        bool minSpirit = UI.currentSpirit - TotalSpiritInPlayArea() > -1;
        if (minSpirit && (CardsInPlayArea() == 1 || _firstOrder == 0 && _lastOrder == 2))
        {
            UI.TogglePlayButton(true);
        }
        else
        {
            UI.TogglePlayButton(false);
        }
    }

    public float CardsInPlayArea()
    {
        int cards = 0;
        for (int i = 0; i < playArea.transform.childCount; i++)
        {
            if (playArea.transform.GetChild(i).childCount > 0)
            {
                cards++;
            }
        }
        return cards;
    }

    public int TotalSpiritInPlayArea()
    {
        int spirit = 0;
        for (int i = 0; i < playArea.transform.childCount; i++)
        {
            if (playArea.transform.GetChild(i).childCount > 0)
            {
                CardData cardData = playArea.transform.GetChild(i).GetChild(0).GetComponent<CardData>();
                int cost = cardData.cost;
                spirit += cost;
            }
        }
        return spirit;
    }
}
