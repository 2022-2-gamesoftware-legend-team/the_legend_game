using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
   private float moveSpeed = 3.0f;
   private Vector3 moveDirecion = Vector3.zero;
   private void Update(){
    float x = Input.GetAxisRaw("Horizontal");
    float y = Input.GetAxisRaw("Vertical");

    moveDirecion = new Vector3(x,y,0);
    transform.position += moveDirecion * moveSpeed * Time.deltaTime;
   }
}
