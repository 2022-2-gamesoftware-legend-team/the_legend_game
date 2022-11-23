using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
   public float maxSpeed;
   public int HP = 5;
   public bool revival = false;
   private float jumpPower = 2.5f;
   private bool isGround;
   private LayerMask groundLayer;
   private Vector3 footPosition;
   private int currentJumpCount = 0;
   private int maxJumpCount = 1;
   private CapsuleCollider2D capsuleCollider2D;

   Rigidbody2D rigid;
   

   
   void Awake(){
      rigid = GetComponent<Rigidbody2D>();
      
      
   }

   void Update(){
      if(Input.GetButtonUp("Horizontal")){
         rigid.velocity = new Vector2(rigid.velocity.normalized.x* 0.5f, rigid.velocity.y);
      }
      if(Input.GetKeyDown(KeyCode.Space)){
         if(currentJumpCount >0){
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            currentJumpCount--;
         }
         
      }
   }

   private void FixedUpdate(){
      // Bounds bounds = capsuleCollider2D.bounds;
      // footPosition = new Vector2(bounds.center.x, bounds.min.y);
      // isGround = Physics2D.OverlapCircle(footPosition, 0.1f, groundLayer);

      // if(isGround == true && rigid.velocity.y <=0){
      //    currentJumpCount = maxJumpCount;
      // }

      float h = Input.GetAxisRaw("Horizontal");
      rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

      if(rigid.velocity.x > maxSpeed){//너무 빠름
         rigid.velocity = new Vector2(maxSpeed,rigid.velocity.y);
      }
      else if(rigid.velocity.x < maxSpeed*(-1)){
         rigid.velocity = new Vector2(maxSpeed*(-1),rigid.velocity.y);
      }
   }
   

   void OnTriggerEnter2D(Collider2D collision){
      if(collision.gameObject.tag=="Item"){
         collision.gameObject.SetActive(false);
         if(HP<5)
            HP ++;
      }
      else if(collision.gameObject.tag=="JumpItem"){
         collision.gameObject.SetActive(false);
         if(maxJumpCount==1){
            maxJumpCount = 2;
         }
         else{
            HP ++;
         }
      }
      else if(collision.gameObject.tag=="revivial"){
         collision.gameObject.SetActive(false);
         if(revival == false ){
            revival = true;
         }
         else{
            HP++;
         }
      }
   }
}
