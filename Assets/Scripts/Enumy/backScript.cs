using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backScript : StateMachineBehaviour
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
        if( Vector2.Distance(enemy.home, enemyTransform.position) <0.1f || Vector2.Distance(enemyTransform.position, enemy.player.position)<=5f)
        {
            animator.SetBool("isBack", false);
            
        }
    
        else
        {
            enemy.DirectionEnemy(enemy.home.x, enemyTransform.position.x);
            enemyTransform.position = Vector2.MoveTowards(enemyTransform.position, enemy.home, Time.deltaTime * enemy.speed);
        }
        

        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
}
