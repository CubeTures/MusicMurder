using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    protected int attackCooldown;
    [SerializeField] GameObject projectile;

    new void Start()
    {
        beatsBetweenActions = 1;
        base.Start();
    }

    protected override void Move()
    {
        if(PlayerIsInLine(4, 0) is Vector2 direction)
        {
            Attack(direction);
        }
        else
        {
            SetDirectionFromPathfinding(PathfindingFallback.DO_NOTHING);
        }
    }

    protected virtual void Attack(Vector2 direction)
    {
        Instantiate(projectile, (Vector2) transform.position + direction, GetQuaternionFromDirection(direction), transform);
    }

    protected override void OnMove(){
        return;
    }
}
