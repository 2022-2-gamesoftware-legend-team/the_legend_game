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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
      {
         if(Vector2.Distance( player.transform.position, bossTransform.position)>5f){
            animator.SetBool("isBack", true);
            animator.SetBool("isFollow", false);
            boss.DirectionBoss(player.transform.position.x, bossTransform.position.x);
            break;
         }
         else if(Vector2.Distance(player.transform.position, bossTransform.position)>1.7f)
             bossTransform.position = Vector2.MoveTowards(bossTransform.position, player.transform.position, Time.deltaTime* boss.speed);
         else
         {
            animator.SetBool("isBack", false);
            animator.SetBool("isFollow", false);
         }
        boss.DirectionBoss(player.transform.position.x, bossTransform.position.x);
      }

        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
}
