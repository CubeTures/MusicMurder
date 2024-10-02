using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularManEnemy : Enemy
{
    int layerMask = 1 << 2;

    protected new void Start()
    {
        beatsBetweenActions = 1;
        base.Start();
        layerMask = ~layerMask;
    }

    protected override void Move()
    {
        Vector2 temp2 = player.currentTile - new Vector2(transform.position.x, transform.position.y);
        Debug.DrawRay(transform.position, temp2, Color.green);
        if(Physics2D.Raycast(transform.position, temp2, Mathf.Min(10f, Vector2.Distance(player.currentTile, new Vector2(transform.position.x, transform.position.y))), layerMask).transform == null)
            direction = pathfinding.GetNextMove();
        else
            direction = GetRandomDirection();
    }

    Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
    }
}
