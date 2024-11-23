using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class VipRoomBoss : Boss, DamageTileCreator
{
    protected Vector2[] directions = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    const int areaAttackLife = 2;
    const int attackCooldown = 2;
    const int
        bubbleAreaWidth = 3, bubbleAreaHeight = 2,
        checkerboardAreaWidth = 5, checkerboardAreaHeight = 2,
        triangleAreaWidth = 3, triangleAreaHeight = 2;
    const string
        bubblePattern = "xxx   ",
        checkerboardPattern = "x x x x x ",
        triangleFrontPattern = "xxx x ",
        triangleLeftPattern = " xx x ",
        triangleRightPattern = "xx  x ";
    protected AreaAttackPattern
        bubbleArea = new(bubbleAreaHeight, bubbleAreaWidth, bubblePattern),
        checkerboardArea = new(checkerboardAreaHeight, checkerboardAreaWidth, checkerboardPattern),
        triangleAreaFront = new(triangleAreaHeight, triangleAreaWidth, triangleFrontPattern),
        triangleAreaLeft = new(triangleAreaHeight, triangleAreaWidth, triangleLeftPattern),
        triangleAreaRight = new(triangleAreaHeight, triangleAreaWidth, triangleRightPattern);

    [SerializeField] GameObject projctile;
    public HashSet<DamageTile> activeProjectiles = new();

    AudioSource audio;
    protected int audioCountdown = -1;

    protected new void Start()
    {
        beatsBetweenActions = 0;
        healths = new int[] { 2, 2, 1 };

        base.Start();

        audio = GetComponent<AudioSource>();
    }

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        base.OnMetronomeBeat(timestamp, failTimestamp, nextBeatTimestamp, startup);
        if (audioCountdown-- == 1)
        {
            audio.Play();
        }
    }

    protected override void Phase1()
    {
        if (PlayerIsInLine(bubbleAreaHeight, bubbleAreaWidth) is Vector2 direction)
        {
            Phase1Attack(direction);
        }
        else
        {
            SetDirectionFromPathfinding();
        }

    }

    protected override void Phase2()
    {
        if (PlayerIsInLine(checkerboardAreaHeight, checkerboardAreaWidth) is Vector2 direction)
        {
            Phase2Attack(direction);
        }
        else
        {
            SetDirectionFromPathfinding();
        }
    }

    protected override void Phase3()
    {
        if (PlayerIsInLine(triangleAreaHeight, triangleAreaWidth) is Vector2 direction)
        {
            Phase3Attack(direction);
            cooldown = 4;
        }
        else
        {
            SetDirectionFromPathfinding();
        }
    }

    protected override void Interphase2()
    {
        ClearProjectiles();
    }

    protected override void Interphase3()
    {
        ClearProjectiles();
    }

    protected override void OnDeath()
    {
        ClearProjectiles();
    }

    protected void Phase1Attack(Vector2 direction)
    {
        HashSet<Vector2> positions = new();

        foreach (Vector2 d in directions)
        {
            positions.AddRange(bubbleArea.GetPositionsSet(transform.position, d));
        }

        CreateDamageTiles(positions);
    }

    protected void Phase2Attack(Vector2 direction)
    {
        HashSet<Vector2> positions = new();

        foreach (Vector2 d in directions)
        {
            positions.AddRange(checkerboardArea.GetPositionsSet(transform.position, d));
        }

        CreateDamageTiles(positions);
    }

    protected void Phase3Attack(Vector2 direction)
    {
        Vector2 left = new(direction.y, -direction.x);
        Vector2 right = -left;

        HashSet<Vector2> positions = new();
        positions.AddRange(triangleAreaFront.GetPositionsSet(transform.position, direction));
        positions.AddRange(triangleAreaLeft.GetPositionsSet(transform.position, left));
        positions.AddRange(triangleAreaRight.GetPositionsSet(transform.position, right));

        CreateDamageTiles(positions);
    }

    protected void CreateDamageTiles(HashSet<Vector2> positions)
    {
        CreateDamageTiles(positions.ToList());
    }

    protected void CreateDamageTiles(List<Vector2> positions)
    {
        foreach (Vector2 position in positions)
        {
            GameObject tileObj = Instantiate(projctile, position, Quaternion.identity, transform);
            DamageTile tile = tileObj.GetComponent<DamageTile>();
            tile.Setup(areaAttackLife);
            activeProjectiles.Add(tile);
        }

        cooldown = attackCooldown;
        audioCountdown = areaAttackLife;
    }

    public void TileDestroyed(DamageTile tile)
    {
        activeProjectiles.Remove(tile);
    }

    void ClearProjectiles()
    {
        foreach (DamageTile tile in activeProjectiles)
        {
            if (tile != null && tile.gameObject != null)
            {
                Destroy(tile.gameObject);
            }
        }

        activeProjectiles.Clear();
    }
}
