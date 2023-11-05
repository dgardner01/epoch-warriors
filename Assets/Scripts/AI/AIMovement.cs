using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{

    public GameObject target;
    public Vector3 direction;
    public float walkSpeed;
    public float knockbackForce;
    public bool stunned;
    public bool attacking;
    Rigidbody2D rb => GetComponent<Rigidbody2D>();

    void Update()
    {
        //calculate direction of target
        direction = (target.transform.position - transform.position).normalized;

        //dont move if stunned or attacking
        if (!attacking && !stunned)
        {
            //move towards target
            rb.velocity = direction * walkSpeed;
        }
        else if (stunned) 
        {
            //if stunned, move away from target
            rb.velocity = -direction * knockbackForce;
        }
        else if (attacking)
        {
            //stop moving if attacking
            rb.velocity = Vector2.zero;
        }
    }
}
