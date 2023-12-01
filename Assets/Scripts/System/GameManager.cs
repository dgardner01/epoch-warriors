using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState {
        Intro,
        FightEnd, 
        CardBegin, 
        CardPlay, 
        CardEnd,
        Fight,
    };
    public float turnTransitionTime;
    bool started;
    public GameState gameState = GameState.Intro;
    public CharacterManager Nelly, Bruttia;
    DialogueManager dialogueManager => FindAnyObjectByType<DialogueManager>();
    CardManager cardManager => FindAnyObjectByType<CardManager>();
    FightManager fightManager => FindAnyObjectByType<FightManager>();
    UIManager UI => FindAnyObjectByType<UIManager>();
    EnemyAI AI => FindAnyObjectByType<EnemyAI>();
    public CameraManager Camera => FindAnyObjectByType<CameraManager>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogueManager.active && gameState == GameState.Intro && !started)
        {
            started = true;
            UI.SetAnimationState(dialogueManager.dialogueAnimator, "up", false);
            StartCoroutine(SwitchGameState());
        }
    }

    public IEnumerator SwitchGameState()
    {
        yield return new WaitForSeconds(turnTransitionTime);
        switch (gameState)
        {
            case GameState.Intro:
                StartCoroutine(CardBegin());
                break;
            case GameState.CardBegin:
                CardPlay();
                break;
            case GameState.CardPlay:
                StartCoroutine(CardEnd());
                break;
            case GameState.CardEnd:
                FightBegin();
                break;
            case GameState.Fight:
                StartCoroutine(CardBegin());
                break;
        }
        //print(gameState);
    }

    public IEnumerator CardBegin()
    {
        gameState = GameState.CardBegin;
        cardManager.DrawCards();
        UI.SetAnimationState(UI.fogAnimator, "up", true);
        Camera.Zoom(4.5f);
        yield return new WaitForSeconds(.25f);
        UI.SetAnimationState(UI.handAnimator, "up", true);
        yield return new WaitForSeconds(1f);
        Nelly.Spirit(3);
        Bruttia.Spirit(3);
        cardManager.UpdatePlayableHand();
        yield return new WaitForSeconds(.15f);
        AI.PlaceCard();
        yield return new WaitForSeconds(1.25f);
        StartCoroutine(SwitchGameState());
    }

    void CardPlay()
    {
        gameState = GameState.CardPlay;
        cardManager.UpdatePlayArea();
    }

    public IEnumerator CardEnd()
    {
        gameState = GameState.CardEnd;
        UI.SetAnimationState(UI.handAnimator, "up", false);
        UI.SetAnimationState(UI.fogAnimator, "up", false);
        Nelly.Spirit(-cardManager.TotalSpiritInPlayArea());
        cardManager.SendCardsToFight();
        cardManager.CleanupPlayAreas();
        yield return new WaitForSeconds(.6f);
        UI.SetAnimationState(UI.playerFightAreaAnimator, "up", true);
        UI.SetAnimationState(UI.enemyFightAreaAnimator, "up", true);
        Camera.Zoom(5);
        StartCoroutine(SwitchGameState());
    }
    public void FightBegin()
    {
        gameState = GameState.Fight;
        StartCoroutine(fightManager.FightSequence());
    }
}
