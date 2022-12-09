using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPriview : MonoBehaviour
{
    SpriteRenderer sp;
    public Sprite Sword;
    public Sprite Spear;
    public Sprite Mace;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectSword()
    {
        sp.sprite = Sword;
    }

    public void selectSpear()
    {
        sp.sprite = Spear;
    }

    public void selectMace()
    {
        sp.sprite = Mace;
    }
}
