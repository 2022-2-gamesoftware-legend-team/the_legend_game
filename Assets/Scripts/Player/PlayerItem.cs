using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
   private PlayerControl playercontrol;

   private void Awake()
    {
        playercontrol = GetComponent<PlayerControl>();

    }
    void OnTriggerEnter2D(Collider2D collision){
      if(collision.gameObject.tag=="Item"){
         collision.gameObject.SetActive(false);
         if(playercontrol.hpPoint<5)
            playercontrol.hpPoint ++;
      }
      else if(collision.gameObject.tag=="JumpItem"){
         collision.gameObject.SetActive(false);
         if(playercontrol.maxJumpcount==1){
            playercontrol.maxJumpcount = 2;
         }
         else{
            playercontrol.hpPoint ++;
         }
      }
      else if(collision.gameObject.tag=="Revival"){
         collision.gameObject.SetActive(false);
         if(playercontrol.revival == false ){
            playercontrol.revival = true;
         }
         else{
            playercontrol.hpPoint++;
         }
      }
    }
}
