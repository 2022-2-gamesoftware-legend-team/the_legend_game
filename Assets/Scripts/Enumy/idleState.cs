using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idleState : StateMachineBehaviour
{
    Transform enemyTransform;
    Enumy enemy;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       enemy = animator.GetComponent<Enumy>();
       enemyTransform = animator.GetComponent<Transform>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
      foreach(GameObject player in players)
      {
         if(Vector2.Distance(enemyTransform.position, player.transform.position)<=4f){
            animator.SetBool("isFollow", true);
            break;
         }
      }
       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

   
}
