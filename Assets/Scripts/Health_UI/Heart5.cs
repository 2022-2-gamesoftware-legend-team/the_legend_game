using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart5 : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Player").GetComponent<PlayerStatus>().HP<=4)
        {
            spriteRenderer.color = new Color(0.2f, 0.2f, 0.2f, 1);
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }

}
