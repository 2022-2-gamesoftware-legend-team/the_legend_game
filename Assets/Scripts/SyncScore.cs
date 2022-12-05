using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncScore : NetworkBehaviour
{
    [SyncVar(hook = nameof(ScoreChanged))]
    public int Score;

    [SyncVar(hook = nameof(MaxScoreChanged))]
    public int MaxScore;

    void Start() {
        print("ScoreManager Start");
    }

    public override void OnStartServer() {
        // When Server started, Load MaxScore from server's PlayerPrefs
        MaxScore = PlayerPrefs.GetInt("MaxScore", 0);
    }

    public void ChangeScore(int score) {
        if (isServer) {
            print("server change score " + score);
            Score = score;
            if (score > MaxScore) {
                MaxScore = Score;
                PlayerPrefs.SetInt("MaxScore", MaxScore);
            }
        } else {
            CmdChangeScore(score);
        }
    }

    [Command(requiresAuthority=false)]
    public void CmdChangeScore(int score) {
        print("client change score " + score);
        ChangeScore(score);
    }

    public void ScoreChanged(int oldScore, int newScore) {
        // UI Update ...
        print("Hook score changed " + newScore);
    }

    public void MaxScoreChanged(int oldMaxScore, int newMaxScore) {
        // UI Update ...
    }
}
