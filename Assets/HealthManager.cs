using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HealthManager : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Heart (1)
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().healthPoint <= 0)
        {
            GameObject.Find("Heart (1)").GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 1);
        }
        else
        {
            GameObject.Find("Heart (1)").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        // Heart (2)
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().healthPoint <= 1)
        {
            GameObject.Find("Heart (2)").GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 1);
        }
        else
        {
            GameObject.Find("Heart (2)").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        // Heart (3)
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().healthPoint <= 2)
        {
            GameObject.Find("Heart (3)").GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 1);
        }
        else
        {
            GameObject.Find("Heart (3)").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        // Heart (4)
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().healthPoint <= 3)
        {
            GameObject.Find("Heart (4)").GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 1);
        }
        else
        {
            GameObject.Find("Heart (4)").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        // Heart (5)
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().healthPoint <= 4)
        {
            GameObject.Find("Heart (5)").GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 1);
        }
        else
        {
            GameObject.Find("Heart (5)").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
}
