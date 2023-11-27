using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCardUI : MonoBehaviour
{
    public GameObject[] cardPrefabs;
    public GameObject playArea;
    public GameObject playerPlayArea;

    public CardManager cardManager => FindAnyObjectByType<CardManager>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyCardPlacement()
    {
        GameObject randomCard = Instantiate(cardPrefabs[Random.Range(0, cardPrefabs.Length)]);
        cardManager.PlaceCard(randomCard, playArea);
    }
}
