using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Grid
{
    public int width { get; private set; }
    public int height { get; private set; }
    float size;
    Vector2Int origin;

    PathNode[,] gridArray;

    public Grid(int width, int height, float size, Vector2Int origin) 
    {
        this.width = width;
        this.height = height;
        this.size = size;
        this.origin = origin;

        gridArray = new PathNode[width, height];
        Init();
    }

    void Init()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridArray[x, y] = new PathNode(this, new Vector2Int(x, y));
            }
        }
    }

    PathNode createNode(Vector2Int pos)
    {
        return new PathNode(this, pos);
    }

    public Vector2 GetWorldPosition(Vector2 pos)
    {
        return pos * size + origin;
    }

    public Vector2Int GetGridPosition(Vector2 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / size) - origin.x;
        int y = Mathf.FloorToInt(worldPosition.y / size) - origin.y;
        return new Vector2Int(x, y);
    }

    public PathNode GetNode(Vector2Int pos)
    {
        return gridArray[pos.x, pos.y];
    }

    public Vector2 GetNodeCenterOffset()
    {
        return new Vector2(size / 2, size / 2);
    }

    public bool Contains(Vector2Int pos)
    {
        int x = pos.x;
        int y = pos.y;
        return x >= 0 && x < width &&
            y >= 0 && y < height;
    }

    public void Reset()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                PathNode n = gridArray[x, y];
                n.ResetNode();
            }
        }
    }

    public void DrawGrid()
    {
        DrawBox(new Vector2Int(0, 0), new Vector2Int(width, height));
    }
    public void DrawNode(PathNode node)
    {
        DrawBox(node.pos, new Vector2Int(node.pos.x + 1, node.pos.y + 1));
    }

    public void DrawList(List<PathNode> list)
    {
        if (list == null)
        {
            Debug.LogWarning("No path to draw");
            return;
        }

        DrawGrid();
        foreach(PathNode node in list)
        {
            DrawNode(node);
        }
    }

    void DrawBox(Vector2Int start, Vector2Int end)
    {
        Vector2 begin = GetWorldPosition(start);
        Vector2 c1 = GetWorldPosition(new Vector2Int(start.x, end.y));
        Vector2 c2 = GetWorldPosition(new Vector2Int(end.x, start.y));
        Vector2 limit = GetWorldPosition(end);

        Debug.DrawLine(begin, c1, Color.red, float.PositiveInfinity);
        Debug.DrawLine(begin, c2, Color.red, float.PositiveInfinity);
        Debug.DrawLine(c1, limit, Color.red, float.PositiveInfinity);
        Debug.DrawLine(c2, limit, Color.red, float.PositiveInfinity);
    }
}
