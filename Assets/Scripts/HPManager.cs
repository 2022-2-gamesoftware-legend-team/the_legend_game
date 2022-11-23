using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    // Start is called before the first frame update
    static int HP = 0;

    public static void setHp(int hp){
        HP += hp;
    }
    public static int getHP(){
        return HP;
    }
}
