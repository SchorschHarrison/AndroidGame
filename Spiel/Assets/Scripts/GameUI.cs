using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    ScoreUIText scoreText;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponentInChildren<ScoreUIText>();
       GameObject.FindGameObjectWithTag("GameEventSystem").GetComponent<GameEventSystem>().onPlayerTargetReached += OnPlayerTargetReached;
    }

    void OnPlayerTargetReached()
    {
        score++;
        scoreText.SetText("" + score);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
