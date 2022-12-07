using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Player player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            GetComponentInParent<Player>().land = true;
        }
    }
}
