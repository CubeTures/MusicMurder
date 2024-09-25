using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    public static PlayerMovement Instance { get; private set; }

    public delegate void PlayerAction(PlayerActionType actionType, float timestamp);
    PlayerAction onPlayerAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Player Instance not Null");
        }
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction.y = 1;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction.y = -1;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction.x = -1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction.x = 1;
        }
    }

    override protected void OnMove()
    {
        NotifyOnPlayerAction(PlayerActionType.MOVE);
    }

    public void ListenOnPlayerAction(PlayerAction p)
    {
        onPlayerAction += p;
    }

    public void UnlistenOnPlayerAction(PlayerAction p)
    {
        onPlayerAction -= p;
    }

    void NotifyOnPlayerAction(PlayerActionType actionType)
    {
        float timestamp = Time.time;
        foreach(PlayerAction p in onPlayerAction.GetInvocationList())
        {
            p.Invoke(actionType, timestamp);
        }
    }
}

public enum PlayerActionType
{
    MOVE,
    ATTACK,
}
