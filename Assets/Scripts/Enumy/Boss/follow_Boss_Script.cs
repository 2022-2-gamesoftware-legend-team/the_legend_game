using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow_Boss_Script : StateMachineBehaviour
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
        if(Vector2.Distance(boss.player.position, bossTransform.position)>5f)
        {
            animator.SetBool("isBack", true);
            animator.SetBool("isFollow", false);
        }
        else if(Vector2.Distance(boss.player.position, bossTransform.position)>1.7f)
            bossTransform.position = Vector2.MoveTowards(bossTransform.position, boss.player.position, Time.deltaTime* boss.speed);
        else
        {
            animator.SetBool("isBack", false);
            animator.SetBool("isFollow", false);
            // bossTransform.position = 0.1f;
        }
        boss.DirectionBoss(boss.player.position.x, bossTransform.position.x);

        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
}
