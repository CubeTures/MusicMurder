using System.Collections.Generic;
using UnityEngine;

public class AreaAttackPattern
{
    private int height;
    private int width;
    private bool[,] pattern, transpose;

    /// <summary>
    /// Initializes an attack pattern where the origin is the top middle. Any whitespace will be read as no attack.
    /// <example>
    /// For example:
    /// <code>
    /// height = 2, width = 3
    /// pattern = " x "
    ///         + "xxx"
    /// </code>
    /// Creates an attack pattern that is one next to the origin, then 3 one tile out.
    /// </example>
    /// </summary>
    public AreaAttackPattern(int height, int width, string pattern)
    {
        this.height = height;
        this.width = width;
        this.pattern = Parse(pattern);
        transpose = Transpose(this.pattern);
    }

    private bool[,] Parse(string pattern)
    {
        char[] chars = pattern.ToCharArray();
        bool[,] area = new bool[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int index = i * width + j;
                bool include = chars[index] != ' ';
                area[i, j] = include;
            }
        }

        return area;
    }

    public List<Vector2> GetPositions(Vector2 origin, Vector2 direction)
    {
        List<Vector2> positions = new();
        List<Vector2> area = GetArea(direction);
        Vector2 offset = GetOffset(direction);

        foreach (Vector2 pos in area)
        {
            positions.Add(pos + offset + origin);
        }

        return positions;
    }

    private string PrintList(List<Vector2> list)
    {
        string result = "[ ";
        foreach (Vector2 pos in list)
        {
            result += $"{pos.ToString()} ";
        }
        result += "]";

        return result;
    }

    private string PrintPattern()
    {
        string result = "";

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                result += pattern[i, j] ? "x" : "_";
            }
            result += "\n";
        }

        return result;
    }

    private List<Vector2> GetArea(Vector2 direction)
    {
        List<Vector2> area = new List<Vector2>();
        int x = 0, y = 0;

        if (direction == Vector2.down)
        {
            for (int j = 0; j < width; j++)
            {
                y = 0;
                for (int i = height - 1; i >= 0; i--)
                {
                    if (pattern[i, j])
                    {
                        area.Add(new Vector2(x, y));
                    }
                    y++;
                }
                x++;
            }
        }
        else if (direction == Vector2.up)
        {
            for (int i = width - 1; i >= 0; i--)
            {
                y = 0;
                for (int j = 0; j < height; j++)
                {
                    if (transpose[i, j])
                    {
                        area.Add(new Vector2(x, y));
                    }
                    y++;
                }
                x++;
            }
        }
        else if (direction == Vector2.right)
        {
            for (int i = 0; i < width; i++)
            {
                y = 0;
                for (int j = 0; j < height; j++)
                {
                    if (transpose[i, j])
                    {
                        area.Add(new Vector2(y, x));
                    }
                    y++;
                }
                x++;
            }
        }
        else if (direction == Vector2.left)
        {
            for (int j = width - 1; j >= 0; j--)
            {
                y = 0;
                for (int i = height - 1; i >= 0; i--)
                {
                    if (pattern[i, j])
                    {
                        area.Add(new Vector2(y, x));
                    }
                    y++;
                }
                x++;
            }
        }
        else
        {
            throw new("Non-Exaustive Pattern Matching ( " + direction + " fell through).");
        }

        return area;
    }

    private Vector2 GetOffset(Vector2 direction)
    {
        if (direction == Vector2.down)
        {
            return new Vector2(-width / 2, -height);
        }
        else if (direction == Vector2.up)
        {
            return new Vector2(-width / 2, 1);
        }
        else if (direction == Vector2.right)
        {
            return new Vector2(1, -width / 2);
        }
        else if (direction == Vector2.left)
        {
            return new Vector2(-height, -width / 2);
        }
        else
        {
            throw new("Non-Exaustive Pattern Matching ( " + direction + " fell through).");
        }
    }

    private T[,] Transpose<T>(T[,] matrix)
    {
        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);

        T[,] result = new T[h, w];

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                result[j, i] = matrix[i, j];
            }
        }

        return result;
    }
}
