using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncItems : NetworkBehaviour
{
    public readonly SyncSortedSet<int> Items = new SyncSortedSet<int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(int item) {
        if (isServer) {
            Items.Add(item);
        } else {
            CmdAddItem(item);
        }
    }

    [Command(requiresAuthority=false)]
    public void CmdAddItem(int item) {
        AddItem(item);
    }

    public bool HasItem(int item) {
        foreach(int i in Items) {
            if (i==item) {
                return true;
            }
        }
        return false;
    }
}
