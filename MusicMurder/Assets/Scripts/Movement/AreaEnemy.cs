using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEnemy : Enemy
{
    protected int areaWidth = 5, areaHeight = 2;
    protected AreaAttackPattern area;
    protected string attackPattern = " xxx "+
                                     "xx xx";
    [SerializeField] GameObject projctile;

    new void Start()
    {
        beatsBetweenActions = 1;
        area = new AreaAttackPattern(areaHeight, areaWidth, attackPattern);
        base.Start();
    }

    protected override void Move()
    {
        if (PlayerIsInLine(areaHeight, areaWidth) is Vector2 direction)
        {
            Attack(direction);
        }
        else
        {
            SetDirectionFromPathfinding(PathfindingFallback.RANDOM_MOVEMENT);
        }
    }

    protected virtual void Attack(Vector2 direction)
    {
        List<Vector2> positions = area.GetPositions(transform.position, direction);
        foreach(Vector2 position in positions)
        {
            Instantiate(projctile, position, Quaternion.identity, transform);
        }
    }
}
