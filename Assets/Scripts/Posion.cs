using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posion : MonoBehaviour
{
    
    public int ItemType;
    public int HP;
    public void OnTriggerEnter2D(Collider other){
        if(other.gameObject.tag =="Player"){
            Movement2D player = other.GetComponent<Movement2D>();
            
        }
    }
    
}
