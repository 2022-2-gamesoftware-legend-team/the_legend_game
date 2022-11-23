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

    public override void OnStartServer() {
        // When Server started, Load MaxScore from server's PlayerPrefs
        MaxScore = PlayerPrefs.GetInt("MaxScore", 0);
    }

    [Command]
    void ChangeScore(int score) {
        Score = score;
        if (score > MaxScore) {
            MaxScore = Score;
            PlayerPrefs.SetInt("MaxScore", MaxScore);
        }
    }

    void ScoreChanged(int oldScore, int newScore) {
        // UI Update ...
    }

    void MaxScoreChanged(int oldMaxScore, int newMaxScore) {
        // UI Update ...
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
