using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{   
    public string category;
    
    void Start()
    {
        int a_RandNum = Random.Range(0, 2); 
        int b_RandNum = Random.Range(0, 100); 
        if(b_RandNum%10==0)
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