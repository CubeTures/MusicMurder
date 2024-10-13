using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrid : MonoBehaviour
{
    public static PlayerGrid Instance { get; private set; }
    public Grid Grid { get; private set; }

    const int MOVE_COST = 1;
    const int WIDTH = 100, HEIGHT = 100;
    const float SIZE = 1;

    PlayerMovement player;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            Grid = new Grid(WIDTH, HEIGHT, SIZE, transform);
            Grid.DrawGrid();
        }
        else
        {
            Debug.LogError("Player Grid Instance not Null");
        }
    }

    private void Start()
    {
        player = PlayerMovement.Instance;
        SetListenStatus(true);
    }

    public void ClearCheckedNodes(PlayerActionType type, float timestamp)
    {
        Grid.ClearCheckedNodes();
    }

    private void Update()
    {
        Grid.DrawGrid();
    }

    void OnEnable()
    {
        SetListenStatus(true);
    }

    void OnDisable()
    {
        SetListenStatus(false);
    }

    void OnDestroy()
    {
        SetListenStatus(false);
    }

    void SetListenStatus(bool status)
    {
        if (player)
        {
            if (status)
            {
                player.ListenOnPlayerAction(ClearCheckedNodes);
            }
            else
            {
                player.UnlistenOnPlayerAction(ClearCheckedNodes);
            }
        }
    }
}
