using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class back_Boss_Script : StateMachineBehaviour
{
    Transform bossTransform;
    Boss boss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       boss = animator.GetComponent<Boss>();
       bossTransform = animator.GetComponent<Transform>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if( Vector2.Distance(boss.home, bossTransform.position) <0.1f || Vector2.Distance(bossTransform.position, boss.player.position)<=5f)
        {
            animator.SetBool("isBack", false);
            
        }
    
        else
        {
            boss.DirectionBoss(boss.home.x, bossTransform.position.x);
            bossTransform.position = Vector2.MoveTowards(bossTransform.position, boss.home, Time.deltaTime * boss.speed);
        }
        

        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
}
