using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriangleAreaEnemy : AreaEnemy
{
    string attackLeft = " xx x ", attackRight = "xx  x ";
    AreaAttackPattern areaLeft, areaRight;

    new void Start()
    {
        Health = 3;
        areaWidth = 3;
        areaHeight = 2;
        attackPattern = "xxx x ";
        attackCooldown = 3;

        base.Start();

        areaLeft = new AreaAttackPattern(areaHeight, areaWidth, attackLeft);
        areaRight = new AreaAttackPattern(areaHeight, areaWidth, attackRight);
    }

    protected override void Attack(Vector2 direction)
    {
        audioCountdown = areaAttackLife;
        Vector2 left = new(direction.y, -direction.x);
        Vector2 right = -left;

        HashSet<Vector2> positions = new();
        positions.AddRange(area.GetPositionsSet(transform.position, direction));
        positions.AddRange(areaLeft.GetPositionsSet(transform.position, left));
        positions.AddRange(areaRight.GetPositionsSet(transform.position, right));

        CreateDamageTiles(positions);
    }
}
