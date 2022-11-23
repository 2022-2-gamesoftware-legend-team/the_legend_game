using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item 
{
    public int itemID; // 아이템 고유 ID 중복 불가
    public string itemName; //아이템 이름 중복 가능
    public string itemDescription; // 아이템 설명
    public int itemCount; 
    public Sprite itemIcon;
    public ItemType itemType;

    public enum ItemType{
        Use,
        Equip
    }

    public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType, int _itemCnt =1){
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDes;
        itemType = _itemType;
        itemCount = _itemCnt;
        itemIcon = Resources.Load(_itemID.ToString(), typeof(Sprite)) as Sprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
