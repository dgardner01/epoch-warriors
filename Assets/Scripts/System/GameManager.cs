using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
        print(gameState);
    }

    void CardBegin()
    {
        gameState = GameState.CardBegin;
        UI.SetAnimationState(UI.fogAnimator, "up", true);
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
    }
}
