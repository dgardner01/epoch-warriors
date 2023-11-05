using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICombat : MonoBehaviour
{
    public bool randomAttack;

    public int enemyLayer;

    public float stunTimer, stunTimerMax;

    public Attack hitAttack;

    public GameObject[] attacks;
    public Transform attackParent;

    Animator animator => GetComponent<Animator>();
    AIMovement movement => GetComponent<AIMovement>();
    private void Update()
    {
        if (randomAttack && !movement.attacking)
        {
            Attack(attacks[Random.Range(0, attacks.Length)]);
            //randomAttack = false;
        }

        //if stunned, stop movement
        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
        }
        else
        {
            movement.stunned = false;
        }

        //if attacking, stop movement
        movement.attacking = attackParent.childCount > 0;

    }
    void Attack(GameObject attack)
    {
        //create an instance of the attack
        GameObject attackInstance = Instantiate(attack, attackParent);
        //make all hitboxes on the same layer as the attacker
        for (int i = 0; i < attackInstance.transform.childCount; i++)
        {
            attackInstance.transform.GetChild(0).gameObject.layer = gameObject.layer;
        }

        //get attack data from instance
        Attack attackData = attackInstance.GetComponent<Attack>();

        //tell the animator it is attacking
        animator.SetTrigger("attack");

        //give animator the attack data

            //attack strength
            switch (attackData.attackStrength)
            {
                case global::Attack.Strength.Light:
                    animator.SetInteger("strength", 0);
                    break;

                case global::Attack.Strength.Medium:
                    animator.SetInteger("strength", 1);
                    break;

                case global::Attack.Strength.Heavy:
                    animator.SetInteger("strength", 2);
                    break;
            }

            //attack area
            switch (attackData.attackArea)
            {
                case global::Attack.Area.High:
                    animator.SetBool("high", true);
                    break;

                case global::Attack.Area.Low:
                    animator.SetBool("high", false);
                    break;
            }

        //point attack towards direction of enemy
        attackInstance.transform.localScale = new Vector3(1 * Mathf.Sign(movement.direction.x), 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if hit by enemy hitbox
        if (collision.gameObject.layer == enemyLayer)
        {
            //get attack data
            Attack attack = collision.gameObject.transform.parent.gameObject.GetComponent<Attack>();

            //stun attacked character
            stunTimer = attack.stunTime;
            movement.knockbackForce = attack.knockback;
            movement.stunned = true;
        }
    }
}
