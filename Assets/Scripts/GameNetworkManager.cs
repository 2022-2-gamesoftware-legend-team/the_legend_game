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

    public float[] GetCameraBoundary() {
        float[] boundary = new float[4]{0,0,0,0};
        string currentScene = networkSceneName;
        if (currentScene.Contains("Stage1"))  {
            boundary[0] = -6;
            boundary[1] = -6;
            boundary[2] = 76;
            boundary[3] = 1;
        } else if (currentScene.Contains("Stage2")) {
            boundary[0] = -10;
            boundary[1] = -6;
            boundary[2] = 43;
            boundary[3] = 2;
        } else if (currentScene.Contains("Stage3")) {
            boundary[0] = -5.9f;
            boundary[1] = -1;
            boundary[2] = 79;
            boundary[3] = 2.4f;
        } else if (currentScene.Contains("Tilemap4")) {
            boundary[0] = -4.3f;
            boundary[1] = -0.3f;
            boundary[2] = 73;
            boundary[3] = 1.9f;
        } else if (currentScene.Contains("Stage5")) {
            boundary[0] = -0.5f;
            boundary[1] = -10;
            boundary[2] = 52;
            boundary[3] = 3.4f;
        }
        return boundary;
    }

    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
        print("Client Scene Changed");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            NetworkIdentity playerIdentity = player.GetComponent<NetworkIdentity>();
            if (playerIdentity.isLocalPlayer) {
                player.GetComponent<Player>().RpcServerSceneChanged();
            }
        }

    }

    public override void OnServerChangeScene(string newSceneName)
    {
        base.OnServerChangeScene(newSceneName);
        print("Server OnServerChangeScene");
    }
}
