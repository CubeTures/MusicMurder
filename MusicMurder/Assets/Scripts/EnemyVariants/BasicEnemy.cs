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
        SetDirectionFromPathfinding();
    }
}

/** Area - fix to not move until attack is done
 * 
 * CheckerboardEnemy (Area attack in cheakerboard pattern, long wind up but hard to avoid)
 * BubbleEnemy (attack around self)
 * TriangleAreaEnemy (attack in triangle, but not as often)
 */