using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckerboardEnemy : AreaEnemy
{
    new void Start(){
        Health = 3;
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
