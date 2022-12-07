using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCleanup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        if (cameras.Length > 1){
            for (int cam=cameras.Length - 1;cam>0;cam--) {
                Destroy(cameras[cam]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
