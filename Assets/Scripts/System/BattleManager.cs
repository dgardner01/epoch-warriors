using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject playerArea;
    public GameObject enemyArea;

    public Animator playerActor;
    public Animator enemyActor;

    public List<CardData.Type> playerActions;
    public List<CardData.Type> enemyActions;
    public List<string> winner;

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
                    if (playerData.cardType == CardData.Type.Block)
                    {
                        enemyCard.GetComponent<Animator>().SetTrigger("hit");
                    }
                    else
                    {
                        playerCard.GetComponent<Animator>().SetTrigger("hit");
                        yield return new WaitForSeconds(0.25f);
                        enemyCard.GetComponent<Animator>().SetTrigger("hurt");
                    }
                    winner.Add("player");
                    yield return new WaitForSeconds(0.25f);
                }
                else if (playerData.cardType == enemyData.cardType)
                {
                    if (playerData.cardType != CardData.Type.Block)
                    {
                        playerCard.GetComponent<Animator>().SetTrigger("hit");
                        enemyCard.GetComponent<Animator>().SetTrigger("hit");
                    }
                    winner.Add("draw");
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    if (enemyData.cardType == CardData.Type.Block)
                    {
                        playerCard.GetComponent<Animator>().SetTrigger("hit");
                        yield return new WaitForSeconds(0.25f);
                    }
                    else
                    {
                        enemyCard.GetComponent<Animator>().SetTrigger("hit");
                        yield return new WaitForSeconds(0.25f);
                        playerCard.GetComponent<Animator>().SetTrigger("hurt");
                    }
                    winner.Add("enemy");
                    yield return new WaitForSeconds(0.25f);
                }
                yield return new WaitForSeconds(0.5f);
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
    public void SetActorStateWithType(Animator actor, CardData.Type type)
    {
        switch (type)
        {
            case CardData.Type.Attack:
                actor.SetTrigger("attack");
                break;
            case CardData.Type.Block:
                actor.SetTrigger("block");
                break;
            case CardData.Type.Grab:
                actor.SetTrigger("grab");
                break;
        }

    }
    public IEnumerator FightScene()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < playerActions.Count; i++)
        {
            if (winner[i] == "player")
            {
                if (playerActions[i] == CardData.Type.Block)
                {
                    print("player blocks");
                    print("enemy attacks");
                    SetActorStateWithType(playerActor, playerActions[i]);
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    print("player does " + playerActions[i]);
                    SetActorStateWithType(playerActor, playerActions[i]);
                    yield return new WaitForSeconds(0.5f);
                }
            }
            else if (winner[i] == "enemy")
            {
                if (enemyActions[i] == CardData.Type.Block)
                {
                    print("enemy blocks");
                    print("player attacks");
                    SetActorStateWithType(playerActor, playerActions[i]);
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    print("enemy does " + enemyActions[i]);
                    yield return new WaitForSeconds(0.5f);
                }
            }
            else
            {
                if (playerActions[i] == CardData.Type.Attack)
                {
                    print("player does " + playerActions[i]);
                    SetActorStateWithType(playerActor, playerActions[i]);
                    yield return new WaitForSeconds(0.5f);
                    print("enemy does " + enemyActions[i]);
                    yield return new WaitForSeconds(0.5f);
                }
                else if (playerActions[i] == CardData.Type.Grab)
                {
                    print("player does " + playerActions[i]);
                    SetActorStateWithType(playerActor, playerActions[i]);
                    print("enemy does " + enemyActions[i]);
                    yield return new WaitForSeconds(0.5f);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

    }
}
