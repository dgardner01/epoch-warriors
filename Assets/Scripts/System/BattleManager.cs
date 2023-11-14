using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject playerArea;
    public GameObject enemyArea;

    public List<CardData.Type> playerActions;
    public List<CardData.Type> enemyActions;

    GameManager gameManager => FindAnyObjectByType<GameManager>();
    CardManager cardManager => FindAnyObjectByType<CardManager>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator CardBattle()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < playerArea.transform.childCount; i++)
        {
            if (playerArea.transform.GetChild(i).childCount > 0 && enemyArea.transform.GetChild(i).childCount > 0)
            {
                print(i);
                GameObject playerCard = playerArea.transform.GetChild(i).GetChild(0).gameObject;
                GameObject enemyCard = enemyArea.transform.GetChild(i).GetChild(0).gameObject;
                CardData playerData = playerCard.GetComponent<CardData>();
                CardData enemyData = enemyCard.GetComponent<CardData>();

                if (playerData.cardType == CardData.Type.Attack && enemyData.cardType == CardData.Type.Grab
                    || playerData.cardType == CardData.Type.Block && enemyData.cardType == CardData.Type.Attack
                    || playerData.cardType == CardData.Type.Grab && enemyData.cardType == CardData.Type.Block)
                {
                    playerCard.GetComponent<Animator>().SetTrigger("hit");
                    yield return new WaitForSeconds(0.3f);
                    enemyCard.GetComponent<Animator>().SetTrigger("hurt");
                    yield return new WaitForSeconds(0.3f);
                }
                else if (playerData.cardType == enemyData.cardType)
                {
                    playerCard.GetComponent<Animator>().SetTrigger("hit");
                    enemyCard.GetComponent<Animator>().SetTrigger("hit");
                    yield return new WaitForSeconds(0.3f);
                }
                else
                {
                    enemyCard.GetComponent<Animator>().SetTrigger("hit");
                    yield return new WaitForSeconds(0.3f);
                    playerCard.GetComponent<Animator>().SetTrigger("hurt");
                    yield return new WaitForSeconds(0.3f);
                }

                playerActions.Add(playerData.cardType);
                enemyActions.Add(enemyData.cardType);
            }
        }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < playerArea.transform.childCount; i++)
        {
            if (playerArea.transform.GetChild(i).childCount > 0)
            {
                Destroy(playerArea.transform.GetChild(i).GetChild(0).gameObject);
            }
            if (playerArea.transform.GetChild(i).childCount > 0)
            {
                Destroy(enemyArea.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        StartCoroutine(gameManager.SwitchGameState());
    }
}
