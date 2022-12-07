using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// �÷��̾��� ���ݸ���� 10%�̻��� �� ����, 100%�̻��̸� �����θ� Destroy
public class AttackCollider : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("Attack Collider Created");
    }

    // Update is called once per frame
    void Update()
    {
        // if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        // {
        //     Destroy(gameObject);
        // }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        print(collider.gameObject.tag);
        print(LayerMask.LayerToName(collider.gameObject.layer));
        if (collider.gameObject.tag == "Enumy") {
            collider.gameObject.GetComponent<Enumy>().Hit();
        }
        if (collider.gameObject.tag == "Boss") {
            collider.gameObject.GetComponent<Boss>().Hit();
        }
        Destroy(gameObject);
    }

    // �� �Ǵ� ������ �浹�� HP -= 1, ���� ����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" &&
           collision.gameObject.tag == "Boss")
        {
            print(collision.gameObject.tag);
            // collision.gameObject.GetComponent<EnemyStatus>().HP -= 1;
            Destroy(gameObject);
        }
    }
}
