using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]

    private SpriteRenderer spriteRenderer;
    public int hpPoint = 5;
    public bool revival = false;
    public int maxJumpcount = 1;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
}
