using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Game State Instance not Null");
        }
    }

    public bool Paused { get; private set; }
    public bool Freeze { get; private set; }

    public void SetPaused(bool state)
    {
        Paused = state; 
    }

    public void SetFreeze(bool state)
    {
        Freeze = state; 
    }
}
