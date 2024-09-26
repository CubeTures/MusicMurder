using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    Pathfinding path;
    Grid grid;

    Transform player;

    void Start()
    {
        path = new Pathfinding(transform.position);
        grid = path.GetGrid();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        grid.DrawGrid();

        List<PathNode> nodes = path.FindPath(transform.position, player.position);
    }
}
