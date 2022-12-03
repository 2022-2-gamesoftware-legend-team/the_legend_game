using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// 플레이어의 공격모션이 10%이상일 때 생성, 100%이상이면 스스로를 Destroy
public class AttackCollider : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Destroy(gameObject);
        }
    }


    // 적 또는 보스와 충돌시 HP -= 1, 판정 삭제
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" &&
           collision.gameObject.tag == "Boss")
        {
            // collision.gameObject.GetComponent<EnemyStatus>().HP -= 1;
            Destroy(gameObject);
        }
    }
}
