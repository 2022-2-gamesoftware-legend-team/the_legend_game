using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followScript : StateMachineBehaviour
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
        if(Vector2.Distance(enemy.player.position, enemyTransform.position)>5f)
        {
            animator.SetBool("isBack", true);
            animator.SetBool("isFollow", false);
        }
        else if(Vector2.Distance(enemy.player.position, enemyTransform.position)>1.7f)
            enemyTransform.position = Vector2.MoveTowards(enemyTransform.position, enemy.player.position, Time.deltaTime* enemy.speed);
        else
        {
            animator.SetBool("isBack", false);
            animator.SetBool("isFollow", false);
            // enemyTransform.position = 0.1f;
        }
        enemy.DirectionEnemy(enemy.player.position.x, enemyTransform.position.x);

        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
}
