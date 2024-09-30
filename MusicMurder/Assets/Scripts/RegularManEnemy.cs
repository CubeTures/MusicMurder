using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularManEnemy : Enemy
{
    protected new void Start()
    {
        beatsBetweenActions = 1;
        base.Start();
    }

    protected override void Move()
    {
        direction = pathfinding.GetNextMove();
        //direction = GetRandomDirection();
    }

    Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
    }
}
