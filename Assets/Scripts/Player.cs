using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Moving
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        Move(dir, Time.deltaTime);
        if (Input.GetButton("Jump")) {
            Jump();
        }
    }
}
