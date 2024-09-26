using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    const int MOVE_COST = 1;
    const int WIDTH = 50, HEIGHT = 50;
    const float SIZE = 1;
    readonly Vector2Int[] directions = new Vector2Int[] 
        { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

    Grid grid;
    List<PathNode> open;
    HashSet<PathNode> closed;

    public Pathfinding(Vector2 origin)
    {
        grid = new Grid(WIDTH, HEIGHT, SIZE, 
            FloorVector(origin) - new Vector2Int(WIDTH / 2, HEIGHT / 2));
    }

    public List<PathNode> FindPath(Vector2 startPos, Vector2 endPos)
    {
        Vector2Int s = grid.GetGridPosition(startPos);
        Vector2Int e = grid.GetGridPosition(endPos);
        return FindPath(s, e);
    }

    public Vector2Int FloorVector(Vector2 vec)
    {
        return new Vector2Int(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.y));
    }

    public List<PathNode> FindPath(Vector2Int startPos, Vector2Int endPos)
    {
        PathNode start = grid.GetNode(startPos);
        PathNode end = grid.GetNode(endPos);
        grid.Reset();

        open = new List<PathNode>() { start };
        closed = new HashSet<PathNode>();

        start.gCost = 0;
        start.hCost = CalculateDistanceCost(start, end);
        start.CalculateFCost();

        while(open.Count > 0)
        {
            PathNode current = GetLowestFCostNode(open);

            if(current.Equals(end))
            {
                return CalculatePath(end);
            }

            open.Remove(current);
            closed.Add(current);

            foreach(PathNode neighbor in GetNeighborList(current))
            {
                if(closed.Contains(neighbor))
                {
                    continue;
                }
                if(!neighbor.isWalkable)
                {
                    closed.Add(neighbor);
                    continue;
                }

                int tentativeGCost = current.gCost + CalculateDistanceCost(current, neighbor);
                if (tentativeGCost < neighbor.gCost)
                {
                    neighbor.previous = current;
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = CalculateDistanceCost(neighbor, end);
                    neighbor.CalculateFCost();

                    if (!open.Contains(neighbor))
                    {
                        open.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    List<PathNode> GetNeighborList(PathNode current)
    {
        List<PathNode> list = new List<PathNode>();

        foreach (Vector2Int direction in directions)
        {
            PathNode n = GetNeighbor(current, direction);

            if(n != null)
            {
                list.Add(n);
            }
        }

        return list;
    }

    PathNode GetNeighbor(PathNode current, Vector2Int direciton)
    {
        Vector2Int v = current.pos + direciton;

        if (grid.Contains(v))
        {
            return grid.GetNode(v);
        }

        return null;
    }

    List<PathNode> CalculatePath(PathNode end)
    {
        List<PathNode> list = new List<PathNode>() { end };

        PathNode current = end;
        while(current.previous != null)
        {
            list.Add(current.previous);
            current = current.previous;
        }

        list.Reverse();
        return list;
    }

    int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int x = Mathf.Abs(a.pos.x - b.pos.x);
        int y = Mathf.Abs(a.pos.y - b.pos.y);
        return MOVE_COST * (x + y);
    }

    PathNode GetLowestFCostNode(List<PathNode> list)
    {
        PathNode lowest = list[0];

        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].fCost < lowest.fCost)
            {
                lowest = list[i];
            }
        }

        return lowest;
    }

    public Grid GetGrid()
    {
        return grid;
    }
}
