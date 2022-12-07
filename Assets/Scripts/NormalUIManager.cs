using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NormalUIManager : MonoBehaviour
{

    SyncScore syncScore;

    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {

        DontDestroyOnLoad(gameObject);
        syncScore = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<SyncScore>();
    }

    // Update is called once per frame
    void Update()
    {
        if (syncScore == null) {
            syncScore = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<SyncScore>();
        } else {
            scoreText.text = "Score: " + syncScore.Score;
        }
    }
}
