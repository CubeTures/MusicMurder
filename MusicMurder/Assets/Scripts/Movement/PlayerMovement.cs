using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Living
{
    public static PlayerMovement Instance { get; private set; }

    public delegate void PlayerAction(PlayerActionType actionType, float timestamp);
    PlayerAction onPlayerAction;
    PlayerTempo tempo;
    public Accuracy acc{get; private set;}

    GameState gameState;
    bool startup = false;

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

    private new void Start(){
        base.Start();
        Health = 3;
        tempo = PlayerTempo.Instance;
        gameState = GameState.Instance;
        tempo.ListenOnPlayerAccuracy(GetAccuracy);
    }

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        this.startup = startup;
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if(gameState.Paused || startup) return;

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

    void GetAccuracy(Accuracy accuracy){
        acc = accuracy;
    }
}

public enum PlayerActionType
{
    MOVE,
    ATTACK,
}
