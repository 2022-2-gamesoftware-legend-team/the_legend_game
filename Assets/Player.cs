using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public string playerName;
    public int healthPoint;
    public float maxSpeed;
    public float jumpPower;

    public Animator anim;
    public Rigidbody2D rigid;
    public SpriteRenderer SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        healthPoint = 5;
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
            if (Input.GetButtonDown("Fire1"))
            {
                anim.SetTrigger("isAttackA");
            }

            // AttackB
            if (Input.GetButtonDown("Fire2"))
            {
                anim.SetTrigger("isAttackB");
            }
        }
    }

    private void FixedUpdate()
    {

    }

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            healthPoint -= 1; // 적과 충돌시 체력 -1;
            anim.SetTrigger("isHit"); // 피격 애니메이션 재생

            if (healthPoint == 0)
            {
                anim.SetBool("isDead", true);
            }
        }

        if (collision.gameObject.tag == "Boss")
        {
            healthPoint -= 2; // 보스와 충돌시 체력 -2;
            anim.SetTrigger("isHit"); // 피격 애니메이션 재생

            if (healthPoint == 0)
            {
                anim.SetBool("isDead", true);
            }
        }

        // 바닥과 접촉시 isJumping = false
        if (collision.gameObject.name == "Tilemap")
        {
            anim.SetBool("isJumping", false);
        }
    }
}
