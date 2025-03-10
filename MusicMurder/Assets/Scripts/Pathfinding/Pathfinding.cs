using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    const int MOVE_COST = 1;
    readonly Vector2Int[][] directions = new Vector2Int[][]
        { new Vector2Int[]{ Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left },
        new Vector2Int[]{ Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down } };

    readonly Grid grid;
    readonly Transform self;
    List<PathNode> open;
    HashSet<PathNode> closed;

    public Pathfinding(Transform self)
    {
        grid = PlayerGrid.Instance.Grid;
        this.self = self;
        grid.DrawNode(grid.CreateNode(self));
        grid.DrawNode(new PathNode(grid, grid.GetTrueOrigin()));
        //grid.DrawNode(new PathNode(grid, grid.GetOrigin()));
    }

    public Vector2Int GetNextMove(Vector2 from)
    {
        return GetNextMove(from, grid.GetTrueOrigin());
    }

    public Vector2Int GetNextMove(Vector2 from, Vector2 point)
    {
        List<PathNode> path = FindPath(from, point);
        if (path != null && path.Count > 1)
        {
            grid.DrawList(path);
            Vector2Int next = path[1].pos - grid.GetGridPosition(self.position);
            //Debug.Log("Next move: " +  next);
            return next;
        }

        //Debug.Log("There is no path from " + self.gameObject.name + " to player");
        return Vector2Int.zero;
    }

    public int GetDistanceToOrigin()
    {
        PathNode nSelf = grid.CreateNode(self);
        PathNode nOrigin = new PathNode(grid, grid.GetTrueOrigin());
        return CalculateDistanceCost(nSelf, nOrigin) / MOVE_COST;
    }

    public Vector2 GetVectorToOrigin()
    {
        PathNode nSelf = grid.CreateNode(self);
        PathNode nOrigin = new PathNode(grid, grid.GetTrueOrigin());
        return GetVectorDistance(nSelf, nOrigin);
    }

    public List<PathNode> FindPath(Vector2 startPos, Vector2 endPos)
    {
        Vector2Int s = grid.GetGridPosition(startPos);
        Vector2Int e = endPos == grid.GetTrueOrigin() ? grid.GetTrueOrigin() : grid.GetGridPosition(endPos);

        if (grid.Contains(s) && grid.Contains(e))
        {
            return FindPath(s, e);
        }

        if (!grid.Contains(s))
        {
            //Debug.LogWarning(self.gameObject.name + " not in grid");
        }
        if (!grid.Contains(e))
        {
            Debug.LogWarning("Endpoint not in grid");
        }

        return null;
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

        while (open.Count > 0)
        {
            PathNode current = GetLowestFCostNode(open);

            if (current.Equals(end))
            {
                return CalculatePath(end);
            }

            open.Remove(current);
            closed.Add(current);
            //grid.DrawNode(current);

            foreach (PathNode neighbor in GetNeighborList(current, end))
            {
                if (closed.Contains(neighbor))
                {
                    continue;
                }
                if (!grid.GetWalkableStatus(neighbor))
                {
                    closed.Add(neighbor);
                    //grid.DrawNode(neighbor);
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

    List<PathNode> GetNeighborList(PathNode current, PathNode end)
    {
        List<PathNode> list = new List<PathNode>();
        int priority = GetDirectionPriority(current, end);

        foreach (Vector2Int direction in directions[priority])
        {
            PathNode n = GetNeighbor(current, direction);

            if (n != null)
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
        while (current.previous != null)
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

    Vector2 GetVectorDistance(PathNode a, PathNode b)
    {
        int x = b.pos.x - a.pos.x;
        int y = b.pos.y - a.pos.y;
        return new Vector2(x, y);
    }

    int GetDirectionPriority(PathNode a, PathNode b)
    {
        int x = Mathf.Abs(a.pos.x - b.pos.x);
        int y = Mathf.Abs(a.pos.y - b.pos.y);

        if (x > y)
        {
            return 1;
        }
        else if (x < y)
        {
            return 0;
        }
        else
        {
            return UnityEngine.Random.Range(0, 2);
        }
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
}
