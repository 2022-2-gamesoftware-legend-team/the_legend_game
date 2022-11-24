using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct CreatePlayerMessage : NetworkMessage
{
    public int type;
}

public class GameNetworkManager : NetworkManager
{

    public int PlayerCharacterType = 1;
    public override void OnStartServer()
    {
        base.OnStartServer();
        // NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreatePlayer);
        print("Server");
        NetworkServer.Spawn(Instantiate(spawnPrefabs[0])); // Score 동기화 GameObject 스폰
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        print("Client");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        CreatePlayerMessage createPlayerMessage = new CreatePlayerMessage
        {
            type = PlayerCharacterType
        };
        NetworkClient.Send(createPlayerMessage);
    }

    void OnCreatePlayer(NetworkConnectionToClient conn, CreatePlayerMessage msg)
    {
        // 선택한 플레이어 스폰
        int spawnPrefabsIndex = -1;
        // set swawnPrefabs index
        switch (msg.type)
        {
            case 1:
                spawnPrefabsIndex = 1;
                break;
            case 2:
                spawnPrefabsIndex = 2;
                break;
            case 3:
                spawnPrefabsIndex = 3;
                break;
            default:
                spawnPrefabsIndex = 1;
                break;
        }
        GameObject gameObject = Instantiate(spawnPrefabs[spawnPrefabsIndex]);
        NetworkServer.AddPlayerForConnection(conn, gameObject);
    }
}
