using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enumy : MonoBehaviour
{
    // public Transform player;
    public bool flip = false;
    Animator animator;
    public float speed = 1.5f;
    private SpriteRenderer spriteRenderer;
    public Vector2 home;

    public float atkCooltime = 3;
    public float atkDelay;
    private Rigidbody2D rigid2D;
    private PlayerControl playercontrol;

    public int enumyHp = 3;
    public bool enumyDie = false;

    public GameObject itemPrefabHp;
    public GameObject itemPrefabRe;
    public GameObject itemPrefabJump;
    private string category ;
    // Start is called before the first frame update
    
        
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        // player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        home = transform.position;
        rigid2D = GetComponent<Rigidbody2D>();
        playercontrol = GetComponent<PlayerControl>();
        
    }
    private void Update()
    {
        
        if(atkDelay >=0)
            atkDelay -= Time.deltaTime;

        if(this.enumyHp <=0 && this.enumyDie == false) //체력이 0이되어서 죽음
        {
            StartCoroutine(Die()); 
        }
        a_RandNum = Random.Range(0, 2); 
        b_RandNum = Random.Range(0, 100); 
    }
    IEnumerator Die() {
        this.animator.SetTrigger("Die");
        this.enumyDie = true;
        this.RandomItem();
        yield return new WaitForSeconds(0.5f);
        this.DropItem();
        Destroy(gameObject);
    }    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("플레이어한테 맞았습니다!");
            Hit();
        }
    }
    
    public void DropItem()
    {
        
        if(this.category =="HpItem")
        {
            Debug.Log("HP");
            var itemGoHp = Instantiate<GameObject>(this.itemPrefabHp);
            itemGoHp.transform.position = this.gameObject.transform.position;
            itemGoHp.SetActive(true);
           
        }
        else if(this.category =="RevivalItem")
        {
            Debug.Log("Revi");
            var itemGoRe = Instantiate<GameObject>(this.itemPrefabRe);
            itemGoRe.transform.position = this.gameObject.transform.position;
            itemGoRe.SetActive(true);
            
        }
        else if(this.category =="jumpItem")
        {
            Debug.Log("Jump");
            var itemGoJump = Instantiate<GameObject>(this.itemPrefabJump);
            itemGoJump.transform.position = this.gameObject.transform.position;
            itemGoJump.SetActive(true);

        }
        else{
            Debug.Log("No Item");
            category = "";
        }
    }    
   public void DirectionEnemy(float target, float baseobj)
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

   public void Hit()
   {
    if(enumyHp>0){
        enumyHp--;
    }
    if(enumyHp<=0 && enumyDie ==false)
    {
        StartCoroutine(Die()); 
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
   public int a_RandNum;
   public int b_RandNum;
   void RandomItem()
    {
        
        if(b_RandNum%2==0)
        {
            if(a_RandNum ==0) //체력 포션
            {
                category = "HpItem";
            }
            else if(a_RandNum ==1) //더블 점프
            {
                category =  "jumpItem";
            }
            else if(a_RandNum ==2) // 부활 점프
            {
                category = "revivalItem";
            }
            else
                category=  "NoItem";
        }
        else
            category =  "NoItem";
    
    }
}
