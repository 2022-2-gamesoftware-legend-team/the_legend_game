using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    public int HP;
    public Color Heart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        HP = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().HP;

        if (HP <= 4)
        {
            GameObject.Find("Heart (5)").GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f);
        }
        else
        {
            GameObject.Find("Heart (5)").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
        }

        if (HP <= 3)
        {
            GameObject.Find("Heart (4)").GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f);
        }
        else
        {
            GameObject.Find("Heart (4)").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
        }

        if (HP <= 2)
        {
            GameObject.Find("Heart (3)").GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f);
        }
        else
        {
            GameObject.Find("Heart (3)").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
        }

        if (HP <= 1)
        {
            GameObject.Find("Heart (2)").GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f);
        }
        else
        {
            GameObject.Find("Heart (2)").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
        }

        if (HP <= 0)
        {
            GameObject.Find("Heart (1)").GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.2f);
        }
        else
        {
            GameObject.Find("Heart (1)").GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
        }
    
        
    }
    
}
