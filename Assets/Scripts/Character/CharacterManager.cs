using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float spirit;
    public float maxSpirit;
    public bool attack;
    GameManager gameManager => FindAnyObjectByType<GameManager>();
    public Animator animator => transform.GetChild(0).GetComponent<Animator>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateAnimatorSpeed();
    }
    void UpdateAnimatorSpeed()
    {
        float slowSpeed = 0.1f;
        float targetSpeed = 1;
        if (gameManager.gameState != GameManager.GameState.Intro && gameManager.gameState != GameManager.GameState.FightBegin)
        {
            if (attack)
            {
                animator.SetTrigger("attack");
                attack = false;
            }
            targetSpeed = 0.005f;
        }
        animator.speed = Mathf.Lerp(animator.speed, targetSpeed, slowSpeed);
    }
    public void Damage(int damage)
    {
        health -= damage;
    }

    public void ReplenishSpirit(int numReplenished)
    {
        spirit += numReplenished;
    }

    public void TriggerAnimation(string state)
    {
        animator.SetTrigger(state);
    }
}
