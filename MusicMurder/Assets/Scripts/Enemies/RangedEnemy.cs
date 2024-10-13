using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    protected int attackCooldown;
    [SerializeField] GameObject projectile;

    new void Start()
    {
        base.Start();
    }

    protected override void Move()
    {

    }
}
