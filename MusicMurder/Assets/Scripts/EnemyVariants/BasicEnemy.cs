/// <summary>
/// Just a basic enemy.
/// </summary>
public class BasicEnemy : Enemy
{
    protected new void Start()
    {
        beatsBetweenActions = 1;
        Health = 3;
        base.Start();
    }

    protected override void Move()
    {
        SetDirectionFromPathfinding();
    }
}