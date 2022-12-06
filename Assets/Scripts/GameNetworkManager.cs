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
        if (GameObject.FindGameObjectWithTag("ScoreManager") == null) {
            print("ScoreManager is not Found. create ScoreManager");
            NetworkServer.Spawn(Instantiate(spawnPrefabs[0])); // Score 동기화 GameObject 스폰
        }
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

    public void NextStage() {
        string currentScene = networkSceneName;
        string nextScene = "Stage1";
        if (currentScene.Contains("Stage1")) {
            nextScene = "Stage2";
        } else if(currentScene.Contains("Stage2")) {
            nextScene = "Stage3";
        } else if(currentScene.Contains("Stage3")) {
            nextScene = "Tilemap4";
        } else if(currentScene.Contains("Tilemap4")) {
            nextScene = "Stage5";
        } else if(currentScene.Contains("Stage5")) {
            print("All Stage Finished.");
        }
        ServerChangeScene("Assets/Scenes/" + nextScene + ".unity");
    }

    public override void OnClientSceneChanged()
    {;
        base.OnClientSceneChanged();
        print("Client Scene Changed");
    }

    public override void OnServerChangeScene(string newSceneName)
    {
        base.OnServerChangeScene(newSceneName);
        print("Server OnServerChangeScene");
    }
}
