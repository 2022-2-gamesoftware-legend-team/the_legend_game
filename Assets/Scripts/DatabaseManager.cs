using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{

    public string[] var_name;
    public float[] var;
    public string[] switch_name;
    public bool[] switchs;

    public List<Item> itemList = new List<Item>();


    // Start is called before the first frame update
    void Start()
    {
        itemList.Add(new Item(1001,"회복 물약", "체력 2칸 회복", Item.ItemType.Use));
        itemList.Add(new Item(1002,"부활 물약", "체력을 모두 회복하고 다시 살아남", Item.ItemType.Use));
        itemList.Add(new Item(1003,"점프 물약", "2단 점프 가능", Item.ItemType.Use));

    }

}
