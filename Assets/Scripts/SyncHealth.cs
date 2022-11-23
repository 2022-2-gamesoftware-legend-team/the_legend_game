using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncHealth : NetworkBehaviour
{

    [SyncVar(hook = nameof(OnHealthChanged))]
    public int health;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    void CmdSetHealth(int health) {
        this.health = health;
    }

    [Command]
    void CmdDecHealth() {
        this.health--;
    }

    [Command]
    void CmdIncHealth() {
        this.health++;
    }

    int CmdGetHealth() {
        return this.health;
    }

    void OnHealthChanged(int oldHealth, int newHealth) {
        // Server Health value change event
        // Client UI Update/etc... 
    }

}
