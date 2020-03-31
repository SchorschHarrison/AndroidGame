using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSystem : MonoBehaviour
{


    void Awake()
    {

    }

    public event Action onPlayerTargetReached;
    public void PlayerTargetReached()
    {
        if(onPlayerTargetReached != null)
        {
            onPlayerTargetReached();
        }
    }

    public event Action onGridStart;
    public void GridStart()
    {
        if(onGridStart != null)
        {
            onGridStart();
        }
    }

    public event Action onResetOrigin;
    public void ResetOrigin()
    {
        if(onResetOrigin != null)
        {
            onResetOrigin();
        }
    }


    public event Action onGameStart;
    public void GameStart()
    {
        if (onGameStart != null)
        {
            onGameStart();
        }
    }

    public event Action onCrash;
    public void Crash()
    {
        if (onCrash != null)
        {
            onCrash();
        }
    }

}
