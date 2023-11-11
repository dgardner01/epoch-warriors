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
        CardBattle,
        CardEnd,
        FightBegin,
    };
    public float turnTransitionTime;
    public GameState gameState = GameState.Intro;
    UIManager UI => FindAnyObjectByType<UIManager>();
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
                Intro();
                break;

            case GameState.CardBegin:
                CardBegin();
                break;
        }
        print(gameState);
    }

    void Intro()
    {
        gameState = GameState.CardBegin;
        StartCoroutine(SwitchGameState());
    }

    void CardBegin()
    {
        gameState = GameState.CardPlay;
        UI.SetAnimationState(UI.fogAnimator, "up", true);
        UI.SetAnimationState(UI.cardsAnimator, "up", true);
        StartCoroutine(SwitchGameState());
    }
}
