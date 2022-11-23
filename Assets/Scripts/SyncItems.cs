using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncItems : NetworkBehaviour
{
    public readonly SyncList<int> Items = new SyncList<int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    void CmdAddItem(int item) {
        Items.Add(item);
    }

    bool HasItem(int item) {
        foreach(int i in Items) {
            if (i==item) {
                return true;
            }
        }
        return false;
    }
}
