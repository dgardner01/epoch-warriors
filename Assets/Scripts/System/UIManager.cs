using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Animator fogAnimator;
    public Animator cardsAnimator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAnimationState(Animator animator, string state, bool status)
    {
        animator.SetBool(state, status);
    }
}
