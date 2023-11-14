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
        EnemyCardPlacement();
    }

    public void EnemyCardPlacement()
    {
        for (int i = 0; i < playArea.transform.childCount; i++)
        {
            Vector3 areaPos = playArea.transform.position;
            areaPos.x = playerPlayArea.transform.position.x;
            transform.position = areaPos;

            Transform _playArea = playArea.transform;
            if (_playArea.GetChild(i).childCount == 0)
            {
                if (i <= cardManager.CardsInPlayArea() - 1)
                {
                    Instantiate(cardPrefabs[Random.Range(0,cardPrefabs.Length)], _playArea.GetChild(i));
                    return;
                }
            }
            else
            {
                if (i > cardManager.CardsInPlayArea() - 1)
                { 
                    Destroy(_playArea.GetChild(i).GetChild(0).gameObject); 
                }
            }
        }
    }

    public IEnumerator RevealCards()
    {
        return null;
    }
}
