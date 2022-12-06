using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public GameManager gameManager;

    // player status
    public string playerName;
    public int HP;
    public float maxSpeed;
    public float jumpPower;
    public bool AttackADone;
    public bool AttackBDone;
    public bool Attacking;
    public bool immune;
    public bool inLadder;

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
        inLadder = false;

        // only Local Player can use Camera Move
        if (isLocalPlayer) {
            if (GetComponent<CameraMove>() == null) {
                float[] cameraBoundary = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<GameNetworkManager>().GetCameraBoundary();
                CameraMove cameraMove = gameObject.AddComponent<CameraMove>();
                cameraMove.minCameraBoundary = new Vector2(cameraBoundary[0], cameraBoundary[1]);
                cameraMove.maxCameraBoundary = new Vector2(cameraBoundary[2], cameraBoundary[3]);
            }
        }
        
        // prevent destroy
        DontDestroyOnLoad(gameObject);
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
            FlipX(rigid.velocity.x < 0);

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

            if (HP > 5)
            {
                HP = 5;
            }
        }
    }

    public void FlipX(bool b)
    {
        if (isServer)
        {
            SpriteRenderer.flipX = b;
        }
        else
        {
            CmdFlipX(b);
        }
    }
    private void FixedUpdate()
    {
        float k = Input.GetAxisRaw("Vertical");
        if (k != 0)
        {
            Ladder(k);
        }
    }

    [Command]
    public void CmdFlipX(bool b)
    {
        SpriteRenderer.flipX = b;
    }

    [ClientRpc]
    public void RpcServerSceneChanged() {
        if (isLocalPlayer) {
            print("Scene Changed. Position Reset");
            Vector3 spawnPosition = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Transform>().position;
            GetComponent<Rigidbody2D>().MovePosition(new Vector2(spawnPosition.x, spawnPosition.y));
            if (GetComponent<CameraMove>() == null) {
                float[] cameraBoundary = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<GameNetworkManager>().GetCameraBoundary();
                CameraMove cameraMove = gameObject.GetComponent<CameraMove>();
                cameraMove.minCameraBoundary = new Vector2(cameraBoundary[0], cameraBoundary[1]);
                cameraMove.maxCameraBoundary = new Vector2(cameraBoundary[2], cameraBoundary[3]);
            }
        }
    }

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            HP -= 1; // ���� �浹�� ü�� -1;
            anim.SetTrigger("isHit"); // �ǰ� �ִϸ��̼� ���

            if (HP == 0)
            {
                anim.SetBool("isDead", true);
            }
        }

        // �ٴڰ� ���˽� isJumping = false
        if (collision.gameObject.name == "Ground")
        {
            anim.SetBool("isJumping", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            gameManager.NextStage();
        }

        if (collision.gameObject.tag == "Ladder")
        {
            inLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            LadderOut();
        }
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

    void Ladder(float k)
    {
        if (this.inLadder)
        {
            rigid.velocity = new Vector2(0, 0);
            rigid.gravityScale = 0;
            this.gameObject.layer = 4;
            if (k > 0)
            {
                Debug.Log("k up");
                rigid.velocity = new Vector2(rigid.velocity.x, maxSpeed);
            }
            if (k < 0)
            {
                Debug.Log("k down");
                rigid.velocity = new Vector2(rigid.velocity.x, maxSpeed * (-1));
            }
        }
    }

    void LadderOut()
    {
        Debug.Log("ladder out");
        this.rigid.gravityScale = 1;
        this.gameObject.layer = 8;
        inLadder = false;
    }



}
