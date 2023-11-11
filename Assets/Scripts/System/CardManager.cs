using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject playArea;
    public GameObject hand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
