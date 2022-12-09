using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestDrop : MonoBehaviour
{
    public int chestHp = 1;
    public bool chestDie = false;

    public int a_RandNum;
    public int b_RandNum;
    public GameObject itemPrefabHp;
    public GameObject itemPrefabRe;
    public GameObject itemPrefabJump;
    private string category ;
    private Animator animator;

    void Awak()
    {
        this.animator = GetComponent<Animator>();
    }


    private void Update()
    {
        
        if(this.chestHp <=0 && this.chestDie == false) //체력이 0이되어서 죽음
        {
            StartCoroutine(Die()); 
        }
        a_RandNum = Random.Range(0, 3); 
        b_RandNum = Random.Range(0, 100); 
    }
    IEnumerator Die() {
        // this.animator.SetBool("isOpen", true);
        this.chestDie = true;
        this.RandomItem();
        yield return new WaitForSeconds(1.5f);
        this.DropItem();
        Destroy(gameObject);
    }    

    public void Hit()
   {
    if(chestHp>0){
        chestHp--;
    }
    if(chestHp<=0 && chestDie ==false)
    {
        StartCoroutine(Die()); 
    }
   }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Hit();
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("상자 열기");
            Hit();
        }
    }
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
                category = "RevivalItem";
            }
            else
                category=  "NoItem";
        }
        else
            category =  "NoItem";
    
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
            Debug.Log("Revival");
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
}
