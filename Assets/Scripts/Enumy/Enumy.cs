using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enumy : MonoBehaviour
{
    public Transform player;
    Animator animator;
    public float speed = 1.5f;
    private SpriteRenderer spriteRenderer;
    public Vector2 home;

    public float atkCooltime = 3;
    public float atkDelay;
    private Rigidbody2D rigid2D;
    // Start is called before the first frame update
    
        
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        home = transform.position;
        rigid2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(atkDelay >=0)
            atkDelay -= Time.deltaTime;
        
    }
    
   public void DirectionEnemy(float target, float baseobj)
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

   public Transform boxpos;
   public Vector2 boxSize;
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
                Debug.Log("Damage");
            }
        }
   }
}
