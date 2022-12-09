using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Boss : NetworkBehaviour
{
    public Transform player;
    public bool flip = false;
    private Animator animator;
    public float speed = 3.5f;
    private SpriteRenderer spriteRenderer;
    public Vector2 home;
    public float atkCooltime = 1f;
    public float atkDelay;

    
    public float skillCooltime = 2f;
    public float skillDelay ;

    public float skill2Cooltime = 2.5f;
    public float skill2Delay ;
    private Rigidbody2D rigid2D;
    private PlayerControl playercontrol;
    public GameObject boss;
    public int bossHp = 10;
    public bool bossDie = false;
    public System.Action onDie;
       ItemDrop itemDrop;

    public GameObject finishPortal;
    // Start is called before the first frame update
    
    
        
    
    void Awake()
    {
        this.animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        home = transform.position;
        rigid2D = GetComponent<Rigidbody2D>();
        playercontrol = GetComponent<PlayerControl>();
        // itemDrop = GetComponent<ItemDrop>();
    }

   
    private void Update()
    {
    
        if(skillDelay>=0)
            skillDelay -= Time.deltaTime;

        if(skill2Delay>=0)
            skill2Delay -= Time.deltaTime;
        
        if(atkDelay >=0)
            atkDelay -= Time.deltaTime;

        if(bossHp <=0&&  bossDie == false)
        {
            StartCoroutine(BossDie());
        }        
    }
    
    IEnumerator BossDie() {
        this.animator.SetTrigger("Die");
        this.bossDie = true;
        // Instantiate(finishPortal, transform.position, transform.rotation);
        CmdCreatePortal();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    [Command(requiresAuthority = false)]
    public void CmdCreatePortal() {
        NetworkServer.Spawn(Instantiate(GameNetworkManager.singleton.spawnPrefabs[4], new Vector3(transform.position.x, -1.0f, transform.position.z), transform.rotation));
    }
    
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.tag == "Player")
    //     {
    //         Debug.Log("플레이어한테 맞았습니다!");
    //         Hit();
    //     }
    // }
    
   public void DirectionBoss(float target, float baseobj)
   {
        if(flip == true) // 우측 시작
        {
            if(target<baseobj)
            {
                animator.SetFloat("Direction", -1);
                spriteRenderer.flipX = false;
            }
            else
            {
                animator.SetFloat("Direction", 1);
                spriteRenderer.flipX = true;
            }
        }
        else //좌측 시작
        {
            if(target<baseobj)
            {
                animator.SetFloat("Direction", -1);
                spriteRenderer.flipX = true;
            }
            else
            {
                animator.SetFloat("Direction", 1);
                spriteRenderer.flipX = false;
            }
        }

        
            
   }

   public Transform boxpos;
   public Vector2 boxSize;

   public void Skill()
   {
    if(animator.GetFloat("Direction")== -1)
    {
        
        if(boxpos.localPosition.x>0)
        {
            boxpos.localPosition = new Vector2(boxpos.localPosition.x * -1,  boxpos.localPosition.y );
        }
    }
    else
    {
        if(boxpos.localPosition.x<0)
        {
            boxpos.localPosition = new Vector2(Mathf.Abs(boxpos.localPosition.x), boxpos.localPosition.y);
        }
    }

    Collider2D[] collider2Ds= Physics2D.OverlapBoxAll(boxpos.position, boxSize, 0);
    foreach(Collider2D coliider in collider2Ds)
    {   
        if(coliider.tag == "Player")
        {
            // playercontrol.hpPoint --;
            Debug.Log("SkillDamage");
        }
    }

   }

  
    public void Hit()
   {
    if(bossHp>0){
        bossHp--;
    }
    if(bossHp<=0 && bossDie ==false)
    {
        StartCoroutine(BossDie()); 
    }
   }
   public void Skill2()
   {
    if(animator.GetFloat("Direction")== -1)
    {
        
        if(boxpos.localPosition.x>0)
        {
            boxpos.localPosition = new Vector2(boxpos.localPosition.x * -1,  boxpos.localPosition.y );
        }
    }
    else
    {
        if(boxpos.localPosition.x<0)
        {
            boxpos.localPosition = new Vector2(Mathf.Abs(boxpos.localPosition.x), boxpos.localPosition.y);
        }
    }

    Collider2D[] collider2Ds= Physics2D.OverlapBoxAll(boxpos.position, boxSize, 0);
    foreach(Collider2D coliider in collider2Ds)
    {   
        if(coliider.tag == "Player")
        {
            // playercontrol.hpPoint --;
            Debug.Log("Skill2Damage");
        }
    }

   }
   public void Attack(){
   
        if(animator.GetFloat("Direction")== -1)
        {
            
            if(boxpos.localPosition.x>0)
            {
                boxpos.localPosition = new Vector2(boxpos.localPosition.x * -1,  boxpos.localPosition.y );
            }
        }
        else
        {
            if(boxpos.localPosition.x<0)
            {
                boxpos.localPosition = new Vector2(Mathf.Abs(boxpos.localPosition.x), boxpos.localPosition.y);
            }
        }

        Collider2D[] collider2Ds= Physics2D.OverlapBoxAll(boxpos.position, boxSize, 0);
        foreach(Collider2D coliider in collider2Ds)
        {   
            if(coliider.tag == "Player")
            {
                // playercontrol.hpPoint --;
                
                Debug.Log("Damage");
            }
        }
   }
}
