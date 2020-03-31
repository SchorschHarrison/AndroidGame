using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameStateManager : MonoBehaviour
{
    public static GameStateManager current;
    
    public static int TUTORIALSTATE = 0;
    public static int PLAYSTATE = 1;
    public static int LOSESTATE = 2;

    public int state;

    private GameEventSystem gameEventSystem;

    private void Awake()
    {
        gameEventSystem = GameObject.FindGameObjectWithTag("GameEventSystem").GetComponent<GameEventSystem>();
        if (GameStateManager.current == null)
        {
            GameStateManager.current = this;
        }
        state = GameStateManager.TUTORIALSTATE;
        gameEventSystem.onGameStart += GameStart;
        gameEventSystem.onCrash += Crash;
    }


    private void GameStart()
    {
        state = GameStateManager.PLAYSTATE;
    }

    private void Crash()
    {
        state = GameStateManager.LOSESTATE;
    }



}
