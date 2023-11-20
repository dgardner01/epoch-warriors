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
        CardBattleBegin,
        CardBattle,
        CardEnd,
        FightBegin,
    };
    public float turnTransitionTime;
    public GameState gameState = GameState.Intro;
    CardManager cardManager => FindAnyObjectByType<CardManager>();
    BattleManager battleManager => FindAnyObjectByType<BattleManager>();
    UIManager UI => FindAnyObjectByType<UIManager>();
    EnemyCardUI AI => FindAnyObjectByType<EnemyCardUI>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SwitchGameState());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SwitchGameState()
    {
        yield return new WaitForSeconds(turnTransitionTime);
        switch (gameState)
        {
            case GameState.Intro:
                CardBegin();
                break;

            case GameState.CardBegin:
                CardPlay();
                break;

            case GameState.CardPlay:
                CardBattleBegin();
                break;

            case GameState.CardBattleBegin:
                CardBattle();
                break;

            case GameState.CardBattle:
                CardEnd();
                break;

            case GameState.CardEnd:
                FightBegin();
                break;

            case GameState.FightBegin:
                FightEnd();
                break;
            case GameState.FightEnd:
                CardBegin();
                break;
        }
        print(gameState);
    }

    void CardBegin()
    {
        gameState = GameState.CardBegin;
        cardManager.DrawCards();
        UI.SetAnimationState(UI.fogAnimator, "up", true);
        UI.SetAnimationState(UI.handAnimator, "up", true);
        UI.SetAnimationState(UI.handAnimator, "up", true);
        StartCoroutine(SwitchGameState());
    }

    void CardPlay()
    {
        gameState = GameState.CardPlay;
    }

    void CardBattleBegin()
    {
        gameState = GameState.CardBattleBegin;
        UI.TogglePlayButton(false);
        UI.SetAnimationState(UI.handAnimator, "up", false);
        UI.SetAnimationState(UI.cardsAnimator, "up", true);
        StartCoroutine(SwitchGameState());
    }

    void CardBattle()
    {
        gameState = GameState.CardBattle;
        StartCoroutine(AI.RevealCards());
    }

    void CardEnd()
    {
        gameState = GameState.CardEnd;
        cardManager.UpdatePlayArea();
        UI.SetAnimationState(UI.fogAnimator, "up", false);
        UI.SetAnimationState(UI.cardsAnimator, "up", false);
        StartCoroutine(SwitchGameState());
    }
    
    void FightBegin()
    {
        gameState = GameState.FightBegin;
        StartCoroutine(battleManager.FightScene());
    }

    void FightEnd()
    {
        gameState = GameState.FightEnd;
        StartCoroutine(SwitchGameState());
    }
}
