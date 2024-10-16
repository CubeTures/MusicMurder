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
        SetDirectionFromPathfinding(PathfindingFallback.RANDOM_MOVEMENT);
    }

    protected override void OnMove(){
        return;
    }
}
