/// <summary>
/// An enemy that acts like the basic enemy but has a more intracate movement pattern.
/// </summary>
public class DancerEnemy : Enemy
{
    readonly int[] movementPattern = { 0, 1 };
    int patternIndex = 0;

    protected new void Start()
    {
        SetNextMove();
        base.Start();
    }

    protected override void Move()
    {
        SetNextMove();
        SetDirectionFromPathfinding(PathfindingFallback.PATROL);
    }

    void SetNextMove()
    {
        beatsBetweenActions = GetNextMove();
    }

    int GetNextMove()
    {
        int val = movementPattern[patternIndex];
        Increment(ref patternIndex, movementPattern.Length);
        return val;
    }

    void Increment(ref int val, int max)
    {
        val++;

        if (val >= max)
        {
            val = 0;
        }
    }
}
