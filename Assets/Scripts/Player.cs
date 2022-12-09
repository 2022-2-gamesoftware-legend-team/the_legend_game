using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public GameManager gameManager;

    // player status
    public string playerName;

    [SyncVar]
    public int HP;

    public readonly SyncSortedSet<int> Items = new SyncSortedSet<int>();
    public float maxSpeed;
    public float socre;
    public float jumpPower;
    public bool AttackADone;
    public bool AttackBDone;
    public bool Attacking;
    public bool Immune;
    public float ImmuneTimer;
    public bool inLadder;
    public bool Jumping;
    public bool DoubleJumpAbllity;
    public bool DoubleJumping;
    public float DoubleJumpDelay;
    public bool land;
    public bool resurrectAbillity;

    // player components
    public Animator anim;
    public Rigidbody2D rigid;
    public SpriteRenderer SpriteRenderer;
    public CircleCollider2D bottom;

    // player attack colliders
    public GameObject AttackAFactroy;
    public GameObject AttackAflipFactroy;
    public GameObject AttackBFactory;
    public GameObject AttackBflipFactory;

    // Scene Change Flag
    bool sceneReloaded = false;

    Vector2 spawnPoint = Vector2.zero;

    // ScoreManagement
    GameObject ScoreManager;
    SyncScore scoreSync;

    // Start is called before the first frame update
    void Start()
    {
        ScoreManager = GameObject.FindGameObjectWithTag("ScoreManager");
        scoreSync = ScoreManager.GetComponent<SyncScore>();
        // HP = 5;
        CmdSetHP(5);
        socre = 0;
        scoreSync.ChangeScore(0);
        
        AttackADone = true;
        AttackBDone = true;
        Attacking = false;
        Immune = false;
        ImmuneTimer = 0.0f;
        inLadder = false;
        DoubleJumpAbllity = false;
        DoubleJumping = false;
        Jumping = false;
        DoubleJumpDelay = 0.0f;
        resurrectAbillity = false;

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
                if (land == true)
                {
                    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    anim.SetBool("isJumping", true);
                    Jumping = true;
                    land = false;
                }
            }
            // DoubleJump
            if (DoubleJumpAbllity == true)
            {
                if (Input.GetButtonDown("Jump") && Jumping == true && DoubleJumpDelay >= 0.01f)
                {
                    Debug.Log("Double Jump");
                    anim.SetBool("isJumping", false); // previous jump motion exit
                    anim.SetBool("isJumping", true); // new jump motion enter
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    Jumping = false;
                    DoubleJumpDelay = 0.0f;
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
                    GameObject AttackACollider = Instantiate(AttackAflipFactroy, transform);
                    // AttackACollider.transform.position = transform.position;
                }
                else
                {
                    GameObject AttackACollider = Instantiate(AttackAFactroy, transform);
                    // AttackACollider.transform.position = transform.position;
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
                    GameObject AttackBCollider = Instantiate(AttackBflipFactory, transform);
                    // AttackBCollider.transform.position = transform.position;
                }
                else
                {
                    GameObject AttackBCollider = Instantiate(AttackBFactory, transform);
                    // AttackBCollider.transform.position = transform.position;
                }

                AttackBDone = true;
            }

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Attacking = false;
            }

            if (HP > 5)
            {
                // HP = 5;
                CmdSetHP(5);
            }

            // immune start

            // immune exit
            if (ImmuneTimer > 1.0f)
            {
                ImmuneTimer = 0.0f;
                CmdSetImmune(false);
                SpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    } 

    public void FlipX(bool b)
    {
        SpriteRenderer.flipX = b;
        if (isClient) {
            CmdFlipX(b);
        }
        if (isServer) {
            RpcFlipX(b);
        }
    }
    private void FixedUpdate()
    {
        float k = Input.GetAxisRaw("Vertical");
        if (k != 0)
        {
            Ladder(k);
        }
        if (isLocalPlayer && sceneReloaded) {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            // GetComponent<Rigidbody2D>().MovePosition(spawnPoint);
            transform.position = new Vector3(spawnPoint.x, spawnPoint.y, transform.position.z);
            print("move spawn positio " + spawnPoint);
            sceneReloaded = false;
        }

        // DoubleJump delay
        if (Jumping == true)
        {
            DoubleJumpDelay = Time.deltaTime;
        }

        if (Immune == true)
        {
            ImmuneTimer += Time.deltaTime;
        }
    }

    [Command]
    public void CmdFlipX(bool b)
    {
        SpriteRenderer.flipX = b;
    }

    [ClientRpc]
    public void RpcFlipX(bool b){
        SpriteRenderer.flipX = b;
    }

    public void ServerSceneChanged(Transform startPosition) {
        if (isLocalPlayer) {
            print("Scene Changed. Position Reset");
            // Vector3 spawnPosition = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<Transform>().position;
            Vector3 spawnPosition = startPosition.position;
            spawnPoint = new Vector2(spawnPosition.x, spawnPosition.y);
            print(spawnPoint);
            if (GetComponent<CameraMove>() != null) {
                float[] cameraBoundary = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<GameNetworkManager>().GetCameraBoundary();
                CameraMove cameraMove = gameObject.GetComponent<CameraMove>();
                cameraMove.minCameraBoundary = new Vector2(cameraBoundary[0], cameraBoundary[1]);
                cameraMove.maxCameraBoundary = new Vector2(cameraBoundary[2], cameraBoundary[3]);
                print(cameraBoundary[0] + " " + cameraBoundary[1] + " " + cameraBoundary[2] + " " + cameraBoundary[3]);
            }
            sceneReloaded = true;
        }
    }

    // [ServerCallback]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enumy" || collision.gameObject.tag == "Boss" && Immune == false)
        {
            // HP -= 1;
            CmdDecHP();
            anim.SetTrigger("isHit");
            SpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            CmdSetImmune(true); // immune timer start

            if (HP <= 0 && resurrectAbillity == false)
            {
                anim.SetBool("isDead", true);
            }
            else if (HP <= 0 && resurrectAbillity == true)
            {
                HP = 5;
            }
        }


        // �ٴڰ� ���˽� isJumping = false
        if (collision.gameObject.name == "Ground" || collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.tag == "Ground")
        {
            anim.SetBool("isJumping", false);
            Jumping = false;
            DoubleJumpDelay = 0.0f;
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

        if(collision.gameObject.tag=="Item"){
            collision.gameObject.SetActive(false);
            if(HP<5)
                HP ++;
            else
            {   
                socre += 100;
                scoreSync.ChangeScore(scoreSync.Score + 100);
            }
        }
        else if(collision.gameObject.tag=="JumpItem"){
            collision.gameObject.SetActive(false);
            if(DoubleJumpAbllity==false){
                DoubleJumpAbllity = true;
            }
            else{
                socre += 200;
                scoreSync.ChangeScore(scoreSync.Score + 200);
            }
        }
        else if(collision.gameObject.tag=="Revival"){
            collision.gameObject.SetActive(false);
            if(Immune == false ){
                Immune = true;
            }
            else{
                socre += 500;
                scoreSync.ChangeScore(scoreSync.Score + 500);
            }
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

    [Command(requiresAuthority = false)]
    void CmdSetHP(int hp) {
        HP = hp;
    }

    [Command(requiresAuthority = false)]
    void CmdSetImmune (bool immune)
    {
        Immune = immune;
    }

    [Command(requiresAuthority = false)]
    void CmdDecHP() {
        HP--;
    }

    [Command(requiresAuthority = false)]
    void CmdIncHP() {
        HP++;
    }

    [Command(requiresAuthority = false)]
    public void CmdAddItem(int item) {
        Items.Add(item);
    }


   

}
