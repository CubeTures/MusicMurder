/// <summary>
/// An enemy that stands still until you approach, where they then rush you.
/// Gets tired after a little while, however.
/// </summary>
public class DrunkardEnemy : Enemy
{
    readonly int rushDuration = 8;
    readonly int restDuration = 8;
    int rushCounter = 0;
    int restCounter = 0;

    protected new void Start()
    {
        beatsBetweenActions = 0;
        base.Start();
    }

    protected override void Move()
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
