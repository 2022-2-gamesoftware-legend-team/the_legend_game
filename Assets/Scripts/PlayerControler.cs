using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    private Movement movement2D;
    private SpriteRenderer spriteRenderer;
    public int hpPoint = 5;
    public bool revival = false;

    private void Awake()
    {
        movement2D = GetComponent<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        spriteRenderer.flipX = (Input.GetAxisRaw("Horizontal")==-1);
        movement2D.Move(x);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            movement2D.Jump();
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
      if(collision.gameObject.tag=="Item"){
         collision.gameObject.SetActive(false);
         if(hpPoint<5)
            hpPoint ++;
      }
      else if(collision.gameObject.tag=="JumpItem"){
         collision.gameObject.SetActive(false);
         if(movement2D.maxJumpcount==1){
            movement2D.maxJumpcount = 2;
         }
         else{
            hpPoint ++;
         }
      }
      else if(collision.gameObject.tag=="revivial"){
         collision.gameObject.SetActive(false);
         if(revival == false ){
            revival = true;
         }
         else{
            hpPoint++;
         }
      }
   }





    
    
}
