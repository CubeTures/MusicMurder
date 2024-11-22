using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleEnemy : AreaEnemy
{
    new void Start()
    {
        beatsBetweenActions = 0;
        attackCooldown = 2;
        areaWidth = 3;
        areaHeight = 2;
        attackPattern = "xxx   ";

        base.Start();
    }

    protected override void Attack(Vector2 direction)
    {
        audioCountdown = areaAttackLife;
        HashSet<Vector2> positions = new();

        foreach (Vector2 d in directions)
        {
            positions.AddRange(area.GetPositionsSet(transform.position, d));
        }

        CreateDamageTiles(positions);
    }
}
