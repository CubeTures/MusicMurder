
using UnityEngine;

public class MainRoomBoss : Boss
{
    [SerializeField] protected GameObject projectile;

    protected new void Start()
    {
        healths = new int[] { 2, 2, 2 };
        base.Start();
    }

    protected override void Phase1()
    {
        SetDirectionFromPathfinding();
    }

    protected override void Phase2()
    {
        if (PlayerIsInLine(15, 0) is Vector2 direction)
        {
            Phase2Attack(direction);
        }
        else
        {
            SetDirectionFromPathfinding();
        }
    }

    void Phase2Attack(Vector2 direction)
    {
        Instantiate(projectile, (Vector2)transform.position + direction,
            GetQuaternionFromDirection(direction), transform);
    }

    protected override void Phase3()
    {
        if (PlayerIsInLine(15, 1) is Vector2 direction)
        {
            Phase3Attack(direction);
        }
        else
        {
            SetDirectionFromPathfinding();
        }
    }

    void Phase3Attack(Vector2 direction)
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

    protected override void Interphase2()
    {
        beatsBetweenActions = 2;
    }

    protected override void Interphase3()
    {
        beatsBetweenActions = 4;
    }

    protected override void OnDeath()
    {
        //throw new System.NotImplementedException();
    }
}
