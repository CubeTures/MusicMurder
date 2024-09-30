using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    const string wallTag = "Walls";
    Grid grid;
    public Vector2Int pos { get; private set; } 

    public int gCost, hCost, fCost;
    public PathNode previous;
    public bool isWalkable;

    public PathNode(Grid grid, Vector2Int pos)
    {
        this.grid = grid;
        this.pos = pos;

        SetWalkable();
        ResetNode();
    }

    public void SetWalkable()
    {
        isWalkable = true;

        Vector2 world = grid.GetWorldPosition(pos) + grid.GetNodeCenterOffset();
        
        if (Physics2D.OverlapPointAll(world) is Collider2D[] colliders)
        {
            foreach(Collider2D collider in colliders)
            {
                if (collider.gameObject.CompareTag(wallTag))
                {
                    isWalkable = false;
                }
            }
        }
    }

    public void ResetNode()
    {
        gCost = int.MaxValue;
        CalculateFCost();
        previous = null;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return "(" + pos.x + "," + pos.y + ")";
    }

    public override bool Equals(object o)
    {
        if (o == null || GetType() != o.GetType())
            return false;

        PathNode other = (PathNode)o;
        return pos == other.pos;
    }

    public override int GetHashCode()
    {
        return pos.GetHashCode();
    }
}
