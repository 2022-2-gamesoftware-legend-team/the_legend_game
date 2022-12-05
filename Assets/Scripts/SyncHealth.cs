using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncHealth : NetworkBehaviour
{

    [SyncVar(hook = nameof(OnHealthChanged))]
    public int Health;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command(requiresAuthority=false)]
    void CmdSetHealth(int health) {
        this.Health = health;
    }

    [Command(requiresAuthority=false)]
    void CmdDecHealth() {
        this.Health--;
    }

    [Command(requiresAuthority=false)]
    void CmdIncHealth() {
        this.Health++;
    }

    int GetHealth() {
        return this.Health;
    }

    void OnHealthChanged(int oldHealth, int newHealth) {
        // Server Health value change event
        // Client UI Update/etc... 
    }

}
