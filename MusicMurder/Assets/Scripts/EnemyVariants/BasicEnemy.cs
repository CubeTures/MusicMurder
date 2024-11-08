/// <summary>
/// Just a basic enemy.
/// </summary>
public class BasicEnemy : Enemy
{
    protected new void Start()
    {
        beatsBetweenActions = 1;
        base.Start();
    }

    protected override void Move()
    {
        SetDirectionFromPathfinding(PathfindingFallback.PATROL);
    }
}
