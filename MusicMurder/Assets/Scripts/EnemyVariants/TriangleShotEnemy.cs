using UnityEngine;

public class TriangleShotEnemy : RangedEnemy
{
    new void Start()
    {
        Health = 2;
        beatsBetweenActions = 2;
        base.Start();
    }

    protected override void Attack(Vector2 direction)
    {
        Vector2[] perp = GetPerpendicular(direction);
        Quaternion rotation = GetQuaternionFromDirection(direction);

        CreateProjectile(direction, rotation);
        CreateProjectile(perp[0], rotation);
        CreateProjectile(perp[1], rotation);
    }

    Vector2[] GetPerpendicular(Vector2 direction)
    {
        if (direction.x != 0)
        {
            return new Vector2[] { Vector2.up, Vector2.down };
        }

        return new Vector2[] { Vector2.left, Vector2.right };
    }

    void CreateProjectile(Vector2 offset, Quaternion rotation)
    {
        Instantiate(projectile, (Vector2)transform.position + offset, rotation, transform);

    }
}
