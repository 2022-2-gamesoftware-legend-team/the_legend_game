using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class selectSwordman : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        prefab = Resources.Load<GameObject>("Prefabs/Swordman");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().playerPrefab = prefab;
    }
}
