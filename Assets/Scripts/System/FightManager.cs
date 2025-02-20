using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public GameObject playerFightArea, enemyFightArea;
    GameManager gameManager => FindAnyObjectByType<GameManager>();
    UIManager UI => FindAnyObjectByType<UIManager>();
    CameraManager cameraManager => FindAnyObjectByType<CameraManager>();
    CharacterManager Nelly => gameManager.Nelly;
    CharacterManager Bruttia => gameManager.Bruttia;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator FightSequence()
    {
        for (int i = 0; i < playerFightArea.transform.childCount; i++)
        {

            Transform playerArea = playerFightArea.transform;
            Transform enemyArea = enemyFightArea.transform;
            Transform playerSlot = playerArea.GetChild(i);
            Transform enemySlot = enemyArea.GetChild(i);

            CardData.Type Attack = CardData.Type.Attack;
            CardData.Type Grab = CardData.Type.Grab;
            CardData.Type Block = CardData.Type.Block;

            if (playerSlot.childCount > 0 && enemySlot.childCount > 0)
            {
                GameObject playerCard = playerSlot.GetChild(0).gameObject;
                GameObject enemyCard = enemySlot.GetChild(0).gameObject;

                CardData playerCardData = playerCard.GetComponent<CardData>();
                CardData enemyCardData = enemyCard.GetComponent<CardData>();

                CardData.Type playerResult = playerCardData.cardType;

                if (playerResult == Block)
                {
                    Nelly.TriggerAnimation("parry");
                    cameraManager.Zoom(4f);
                    yield return new WaitForSeconds(0.25f);
                    cameraManager.FreezeThenShake(.3f, 0.01f, .5f);
                    Nelly.Damage(1);
                    playerCard.GetComponent<Animator>().SetTrigger("discard");
                    enemyCard.GetComponent<Animator>().SetTrigger("discard");
                    yield return new WaitForSeconds(0.5f);
                    cameraManager.Zoom(5f);
                }
                else if (playerResult == Grab)
                {
                    Nelly.TriggerAnimation("hurt");
                    cameraManager.Zoom(4f);
                    yield return new WaitForSeconds(0.25f);
                    cameraManager.FreezeThenShake(.3f, 0.01f, 1f);
                    Nelly.Damage(2);
                    playerCard.GetComponent<Animator>().SetTrigger("discard");
                    enemyCard.GetComponent<Animator>().SetTrigger("discard");
                    yield return new WaitForSeconds(0.5f);
                    cameraManager.Zoom(5f);
                }
                else if (playerResult == Attack)
                {
                    Nelly.TriggerAnimation("hurt");
                    cameraManager.Zoom(4f);
                    yield return new WaitForSeconds(0.25f);
                    cameraManager.FreezeThenShake(.3f, 0.01f, 1f);
                    Nelly.Damage(2);
                    enemyCard.GetComponent<Animator>().SetTrigger("discard");
                    yield return new WaitForSeconds(0.5f);
                    cameraManager.Zoom(5f);
                    Nelly.TriggerAnimation("attack");
                    yield return new WaitForSeconds(0.1f);
                    Bruttia.TriggerAnimation("hurt");
                    cameraManager.Zoom(4f);
                    yield return new WaitForSeconds(0.15f);
                    cameraManager.FreezeThenShake(.3f * playerCardData.damage, 0.01f, .3f * playerCardData.damage);
                    Bruttia.Damage(playerCardData.damage);
                    playerCard.GetComponent<Animator>().SetTrigger("discard");
                    yield return new WaitForSeconds(0.5f);
                    cameraManager.Zoom(5f);
                }
            }
            else if (playerSlot.childCount > 0 && enemySlot.childCount == 0)
            {
                GameObject playerCard = playerSlot.GetChild(0).gameObject;

                CardData playerCardData = playerCard.GetComponent<CardData>();

                CardData.Type playerResult = playerCardData.cardType;

                if (playerResult == Attack)
                {
                    Nelly.TriggerAnimation("attack");
                    yield return new WaitForSeconds(0.1f);
                    Bruttia.TriggerAnimation("hurt");
                    cameraManager.Zoom(4f);
                    yield return new WaitForSeconds(0.15f);
                    cameraManager.FreezeThenShake(.3f * playerCardData.damage, 0.01f, .3f * playerCardData.damage);
                    Bruttia.Damage(playerCardData.damage);
                    playerCard.GetComponent<Animator>().SetTrigger("discard");
                    yield return new WaitForSeconds(0.5f);
                    cameraManager.Zoom(5f);
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        UI.SetAnimationState(UI.playerFightAreaAnimator, "up", false);
        UI.SetAnimationState(UI.enemyFightAreaAnimator, "up", false);
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < playerFightArea.transform.childCount; i++)
        {
            if (playerFightArea.transform.GetChild(i).childCount > 0)
            {
                Destroy(playerFightArea.transform.GetChild(i).GetChild(0).gameObject);
            }
            if (enemyFightArea.transform.GetChild(i).childCount > 0)
            {
                Destroy(enemyFightArea.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        gameManager.Bruttia.attack = true;
        StartCoroutine(gameManager.SwitchGameState());
    }
}
