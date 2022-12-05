using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    // player status
    public string playerName;
    public int HP;
    public float maxSpeed;
    public float jumpPower;
    public bool AttackADone;
    public bool AttackBDone;
    public bool Attacking;
    public bool immune;

    // player components
    public Animator anim;
    public Rigidbody2D rigid;
    public SpriteRenderer SpriteRenderer;

    // player attack colliders
    public GameObject AttackAFactroy;
    public GameObject AttackAflipFactroy;
    public GameObject AttackBFactory;
    public GameObject AttackBflipFactory;

    // Start is called before the first frame update
    void Start()
    {
        HP = 5;
        AttackADone = true;
        AttackBDone = true;
        Attacking = false;
        immune = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            // Walk
            float h = Input.GetAxisRaw("Horizontal");

            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            if (rigid.velocity.x > maxSpeed)
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            else if (rigid.velocity.x < maxSpeed * (-1))
                rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

            if (Input.GetButtonUp("Horizontal"))
            {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
            }

            // Walk_animation
            if (Input.GetButtonDown("Horizontal"))
            {
                SpriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
            }

            if (rigid.velocity.normalized.x == 0)
                anim.SetBool("isWalking", false);
            else
            {
                anim.SetBool("isWalking", true);
            }

            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                if (anim.GetBool("isJumping") == false)
                {
                    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    anim.SetBool("isJumping", true);
                }
            }


            // AttackA
            if (Input.GetButtonDown("Fire1") && AttackADone == true && Attacking == false)
            {
                anim.SetTrigger("isAttackA");
                AttackADone = false;
                Attacking = true;
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("AttackA") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f &&
                AttackADone == false)
            {
                if (SpriteRenderer.flipX == true)
                {
                    GameObject AttackACollider = Instantiate(AttackAflipFactroy);
                    AttackACollider.transform.position = transform.position;
                }
                else
                {
                    GameObject AttackACollider = Instantiate(AttackAFactroy);
                    AttackACollider.transform.position = transform.position;
                }

                AttackADone = true;
            }

            // AttackB
            if (Input.GetButtonDown("Fire2") && AttackBDone == true && Attacking == false)
            {
                anim.SetTrigger("isAttackB");
                AttackBDone = false;
                Attacking = true;
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("AttackB") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f &&
                AttackBDone == false)
            {
                if (SpriteRenderer.flipX == true)
                {
                    GameObject AttackBCollider = Instantiate(AttackBflipFactory);
                    AttackBCollider.transform.position = transform.position;
                }
                else
                {
                    GameObject AttackBCollider = Instantiate(AttackBFactory);
                    AttackBCollider.transform.position = transform.position;
                }

                AttackBDone = true;
            }

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Attacking = false;
            }

            if(HP > 5)
            {
                HP = 5;
            }
        }
    }

    private void FixedUpdate()
    {

    }

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            HP -= 1; // 적과 충돌시 체력 -1;
            anim.SetTrigger("isHit"); // 피격 애니메이션 재생

            if (HP == 0)
            {
                anim.SetBool("isDead", true);
            }
        }

        // 바닥과 접촉시 isJumping = false
        if (collision.gameObject.name == "Ground")
        {
            anim.SetBool("isJumping", false);
        }
    }
}
