using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyAnimate : NetworkBehaviour
{

    public Animator enemyAnimator;
    public Rigidbody2D enemyRigidbody;

    [Header("Movement")]
    public float speed;
    public float duration;
    float current;
    int action;

    // Start is called before the first frame update
    void Start()
    {
        action = 0;
        current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            float horizontal = Input.GetAxis("Horizontal");
            Vector2 moving = (Vector2.right * horizontal * speed);
            Vector2 jumping = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.Space) && !enemyAnimator.GetBool("isJumping")) {
                // && !enemyAnimator.GetBool("isJumping")
                print("jump");
                // enemyRigidbody.velocity += Vector2.up * speed;
                enemyAnimator.SetBool("isJumping", true);
                // enemyRigidbody.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
                jumping = Vector2.up * speed;
            } else if (!enemyAnimator.GetBool("isJumping") && Input.GetKeyDown(KeyCode.LeftControl)) {
                enemyAnimator.SetTrigger("Attack");
            }
            if (moving != Vector2.zero || jumping != Vector2.zero) {
                // CmdMove(moving + jumping);
                enemyRigidbody.AddForce(moving + jumping, ForceMode2D.Impulse);
            }
            enemyAnimator.SetBool("isWalking", moving != Vector2.zero);
        }
    }

    void FixedUpdate() {
        if (enemyRigidbody.velocity.x > speed) {
            enemyRigidbody.velocity = new Vector2(speed, enemyRigidbody.velocity.y);
        }else if (enemyRigidbody.velocity.x < -speed) {
            enemyRigidbody.velocity = new Vector2(-speed, enemyRigidbody.velocity.y);
        }
        if (!enemyAnimator.GetBool("isWalking")) {
            enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
        }
    }
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }
        print("oncollisionenter2d");
        enemyAnimator.SetBool("isJumping", false);
    }
}
