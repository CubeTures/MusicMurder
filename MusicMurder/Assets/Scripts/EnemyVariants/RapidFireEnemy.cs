public class RapidFireEnemy : RangedEnemy
{
    readonly int[] movementPattern = { 0, 1 };
    int patternIndex = 0;

    protected new void Start()
    {
        Health = 2;
        SetNextMove();
        base.Start();
    }

    protected override void Move()
    {
        SetNextMove();
        base.Move();
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
