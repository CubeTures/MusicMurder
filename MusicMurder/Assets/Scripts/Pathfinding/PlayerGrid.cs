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

    private void Update()
    {
        Grid.DrawGrid();
    }
}
