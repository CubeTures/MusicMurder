using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Movement
{
    GameState gameState;
    const string playerTag = "Player";
    protected int beatsBetweenActions = 0;
    protected PlayerMovement player;
    protected Pathfinding pathfinding;
    int beatsSinceAction = 0;

    readonly int layerMask = ~(1 << 2);
    public Health Health { get; protected set; }

    static Dictionary<Vector2Int, Enemy> enemyMap = new Dictionary<Vector2Int, Enemy>();


    protected new void Start()
    {
        gameState = GameState.Instance;
        player = PlayerMovement.Instance;

        base.Start();

        Health = new Health(3);
        pathfinding = new Pathfinding(transform);
    }

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        if (gameState.Paused || startup) return;

        Increment(ref beatsSinceAction, beatsBetweenActions);
        if (beatsSinceAction == beatsBetweenActions)
        {
            Move();
            Vector2Int temp = new Vector2Int(Mathf.CeilToInt(getNext().x), Mathf.CeilToInt(getNext().y));
            if (!enemyMap.ContainsKey(temp)){
                enemyMap.Add(temp, this);
            }
            else {
                colliding = true;
            }
        }
        else {
            Vector2Int temp = new Vector2Int(Mathf.CeilToInt(getNext().x), Mathf.CeilToInt(getNext().y));
            if (!enemyMap.ContainsKey(temp)){
                enemyMap.Add(temp, this);
            }
            else{
                ChainCancel(temp);
                // enemyMap.Add(temp, this);
            }
        }
    }

    protected abstract void Move();

    protected void SetDirectionFromPathfinding(PathfindingFallback fallback = PathfindingFallback.DO_NOTHING)
    {
        if(PlayerInLineOfSight())
        {
            direction = pathfinding.GetNextMove();
        }
        else if(fallback == PathfindingFallback.RANDOM_MOVEMENT)
        {
            direction = GetRandomDirection();
        }
        else if(fallback == PathfindingFallback.FOLLOW_WAYPOINTS)
        {
            // follow pretedermined path
        }
        else if(fallback == PathfindingFallback.PATROL)
        {
            // patrol a predermined spot
        }
    }

    protected bool PlayerInLineOfSight()
    {
        return GetPlayerRaycast().transform == null;
    }

    protected RaycastHit2D GetPlayerRaycast()
    {
        Vector2 direction = player.currentTile - new Vector2(transform.position.x, transform.position.y);
        float distance = Mathf.Min(10f, Vector2.Distance(player.currentTile, new Vector2(transform.position.x, transform.position.y)));
        return Physics2D.Raycast(transform.position, direction, distance, layerMask);
    }

    protected Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
    }

    /// <summary>
    /// Returns the direction to the player when the player is in the given line and not blocked by walls, otherwise returns null
    /// </summary>
    /// <param name="distance">The max distance from the enemy the player can be and still be detected</param>
    /// <param name="spread">The distance perpendicular to the line the player can be and still be detected</param>
    protected Vector2? PlayerIsInLine(int distance, int spread)
    {
        if(!PlayerInLineOfSight()) return null;

        Vector2 v = GetDistanceFromPlayer();

        float x = Mathf.Abs(v.x);
        float y = Mathf.Abs(v.y);
        float line = Mathf.Max(x, y);
        float offset = Mathf.Min(x, y);

        if(line <= distance && offset <= spread)
        {   
            return GetUnitDirection(v);
        }

        return null;
    }

    protected Quaternion GetQuaternionFromDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float clamped = Mathf.Round(angle / 90f) * 90f;
        return Quaternion.Euler(0, 0, clamped);
    }

    Vector2 GetUnitDirection(Vector2 direction)
    {
        if(direction == Vector2.zero) return Vector2.zero;

        float a = Mathf.Abs(direction.x);
        float b = Mathf.Abs(direction.y);

        if (a > b)
        {
            if (direction.x < 0)
            {
                return Vector2.left;
            }
            else
            {
                return Vector2.right;
            }
        }
        else
        {
            if (direction.y < 0)
            {
                return Vector2.down;
            }
            else
            {
                return Vector2.up;
            }
        }
    }

    protected Vector2 GetDistanceFromPlayer()
    {
        return pathfinding.GetVectorToOrigin();
    }

    void Increment(ref int val, int max)
    {
        val++;
        if (val > max)
        {
            val = 0;
        }
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            if(!isMoving && player.acc != Accuracy.FAIL){
                Health.TakeDamage(1);
                Hurt();
                player.CancelMoveCollide();
            }else{
                player.Health.TakeDamage(1);
                player.Hurt();
                player.CancelMoveCollide();
                if(isMoving)
                    ChainCancel(new Vector2Int(Mathf.CeilToInt(getNextPrime().x), Mathf.CeilToInt(getNextPrime().y)));
            }
            if(Health.GetHealth() <= 0){
                DestroyEnemy();
            }
            if(player.Health.GetHealth() <= 0){
                Debug.Log("Player died");
            }
        }

        base.OnCollisionEnter2D(collision);
    }

    void ChainCancel(Vector2Int v) {
        Debug.Log("One " + v);
        Vector2Int chain = new Vector2Int(Mathf.CeilToInt(enemyMap[v].currentTile.x), Mathf.CeilToInt(enemyMap[v].currentTile.y));
        Debug.Log("Chain" + enemyMap[v] + " " + enemyMap[v].currentTile);
        Enemy tempEnemy = enemyMap[v];
        tempEnemy.CancelMoveCollide();
        enemyMap.Remove(v);

        if(enemyMap.ContainsKey(chain)){
            ChainCancel(chain);
            if(!enemyMap.ContainsKey(chain))
                enemyMap.Add(chain, tempEnemy);
        }else{
            enemyMap.Add(chain, tempEnemy);
        }
    }

    protected override void RemoveFromMap()
    {
        enemyMap.Remove(new Vector2Int(Mathf.CeilToInt(getNextPrime().x), Mathf.CeilToInt(getNextPrime().y)));
    }

    protected override void RemoveFromMapPrime() //Yes I know the names are swapped don't touch it
    {
        enemyMap.Remove(new Vector2Int(Mathf.CeilToInt(getNext().x), Mathf.CeilToInt(getNext().y)));
    }

    void DestroyEnemy()
    {
        Debug.Log("Enemy Died");
        Destroy(gameObject);
    }
}

public enum PathfindingFallback
{
    DO_NOTHING,
    RANDOM_MOVEMENT,
    FOLLOW_WAYPOINTS,
    PATROL
}
