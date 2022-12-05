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
        a_RandNum = Random.Range(0, 2); 
        b_RandNum = Random.Range(0, 100); 
    }
    IEnumerator Die() {
        // this.animator.SetBool("isOpen", true);
        this.chestDie = true;
        this.RandomItem();
        yield return new WaitForSeconds(0.5f);
        this.DropItem();
        Destroy(gameObject);
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
                category = "revivalItem";
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
