 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ready_Boss_Script : StateMachineBehaviour
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
        if(boss.skillDelay<=0)
                animator.SetTrigger("Skill1");
        
        if(boss.skill2Delay<=0)
            animator.SetTrigger("Skill2");    

        if(boss.atkDelay<=0)
            animator.SetTrigger("Attack");
        if(Vector2.Distance(boss.player.position, bossTransform.position)> 3f)
            animator.SetBool("isFollow",true);
        boss.DirectionBoss(boss.player.position.x, bossTransform.position.x);

        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
}
