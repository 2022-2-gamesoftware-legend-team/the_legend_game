using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSceneChange : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)){
            CmdNextStage();
        }
    }

    [Command(requiresAuthority = false)]
    void CmdNextStage() {
        GameNetworkManager gameNetworkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<GameNetworkManager>();
        if (gameNetworkManager) {
            print("OnTriggerEnter2D NextStage");
            gameNetworkManager.NextStage();
        } else {
            print("Error: GameNetworkManager not found.");
        }
    }

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Finish") {
            GameNetworkManager gameNetworkManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<GameNetworkManager>();
            if (gameNetworkManager) {
                print("OnTriggerEnter2D NextStage");
                gameNetworkManager.NextStage();
            } else {
                print("Error: GameNetworkManager not found.");
            }
        }
    }
}
