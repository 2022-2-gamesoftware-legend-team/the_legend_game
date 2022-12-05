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
        NetworkServer.RegisterHandler<CreatePlayerMessage>(OnCreatePlayer);
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

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        print(sceneName);
    }

    void OnCreatePlayer(NetworkConnectionToClient conn, CreatePlayerMessage msg)
    {
        // 선택한 플레이어 스폰
        GameObject player = playerPrefab;
        // set swawnPrefabs index
        switch (msg.type)
        {
            case 1:
                break;
            case 2:
                player = spawnPrefabs[1];
                break;
            case 3:
                player = spawnPrefabs[2];
                break;
            default:
                break;
        }
        GameObject playerObject = Instantiate(player);
        NetworkServer.AddPlayerForConnection(conn, playerObject);
    }

    public void SelectPlayer(int player) {
        PlayerCharacterType = player;
    }
}
