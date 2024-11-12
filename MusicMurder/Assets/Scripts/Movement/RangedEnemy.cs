using UnityEngine;

public class RangedEnemy : Enemy
{
    protected int attackCooldown;
    protected int sightRange = 10;
    [SerializeField] protected GameObject projectile;

    new void Start()
    {
        beatsBetweenActions = 1;
        base.Start();
    }

    protected override void Move()
    {
        if (PlayerIsInLine(sightRange, 0) is Vector2 direction)
        {
            Attack(direction);
        }
        else
        {
            SetDirectionFromPathfinding();
        }
    }

    protected virtual void Attack(Vector2 direction)
    {
        Instantiate(projectile, (Vector2)transform.position + direction, GetQuaternionFromDirection(direction), transform);
    }
}
