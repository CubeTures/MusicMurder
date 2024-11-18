public class StoreroomBoss : Boss
{
    readonly int[] movementPattern = { 0, 1 };
    int patternIndex = 0;

    readonly int rushDuration = 8;
    readonly int restDuration = 8;
    int rushCounter = 0;
    int restCounter = 0;

    protected new void Start()
    {
        healths = new int[] { 1, 1, 1 };
        beatsBetweenActions = 1;
        base.Start();
    }

    protected override void Phase1()
    {
        SetDirectionFromPathfinding();
    }

    protected override void Phase2()
    {
        SetNextMove();
        SetDirectionFromPathfinding();
    }

    protected override void Phase3()
    {
        if (Rushing())
        {
            SetDirectionFromPathfinding(PathfindingFallback.DO_NOTHING);
        }

        if (playerSighted > 0)
        {
            if (Rushing())
            {
                rushCounter++;
            }
        }
        else
        {
            if (rushCounter > 0)
            {
                rushCounter = rushDuration;
            }
        }

        if (Resting())
        {
            restCounter++;
        }
        else if (!Rushing())
        {
            Reset();
        }
    }

    protected override void Interphase2()
    {
        beatsBetweenActions = 0;
    }

    protected override void Interphase3()
    {
        beatsBetweenActions = 0;
    }

    protected override void OnDeath()
    {

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

    bool Rushing()
    {
        return rushCounter < rushDuration && restCounter == 0;
    }

    bool Resting()
    {
        return rushCounter >= rushDuration && restCounter < restDuration;
    }

    private void Reset()
    {
        rushCounter = 0;
        restCounter = 0;
    }
}
