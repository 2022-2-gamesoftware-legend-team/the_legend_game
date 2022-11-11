using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimate : MonoBehaviour
{

    Animator enemyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
