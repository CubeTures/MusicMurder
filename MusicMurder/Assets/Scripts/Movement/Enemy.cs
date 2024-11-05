using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Enemy : Living
{
    const string playerTag = "Player";
    protected int beatsBetweenActions = 0;
    protected PlayerMovement player;
    protected Pathfinding pathfinding;
    int beatsSinceAction = 0;
    int playerSighted = 0;
    PlayerTempo playerTempo;

    [SerializeField] GameObject deathAnimation;
    [SerializeField] GameObject curtain;

    readonly int layerMask = ~(1 << 2);

    public static Dictionary<Vector2Int, Enemy> enemyMap = new Dictionary<Vector2Int, Enemy>();
    Vector2 startingPoint;
    [SerializeField] Transform[] waypoints;
    int waypointIndex = 0;

    protected new void Start()
    {
        player = PlayerMovement.Instance;
        playerTempo = PlayerTempo.Instance;

        base.Start();

        Health = 3;
        pathfinding = new Pathfinding(transform);
        startingPoint = transform.position;
    }

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        if (gameState.Paused || startup) return;

        Increment(ref beatsSinceAction, beatsBetweenActions);
        if (beatsSinceAction == beatsBetweenActions)
        {
            Move();
            Vector2Int temp = new Vector2Int(
                Mathf.CeilToInt(getNext().x),
                Mathf.CeilToInt(getNext().y));

            if (!enemyMap.ContainsKey(temp))
            {
                enemyMap.Add(temp, this);
                // does this ever get removed?
            }
            else
            {
                print($"Colliding with existing: {enemyMap[temp]}");
                colliding = true;
            }
        }
        else
        {
            Vector2Int temp = new Vector2Int(Mathf.CeilToInt(getNext().x), Mathf.CeilToInt(getNext().y));
            if (!enemyMap.ContainsKey(temp))
            {
                enemyMap.Add(temp, this);
            }
            else
            {
                ChainCancel(temp);
            }
        }
    }

    protected abstract void Move();

    protected void SetDirectionFromPathfinding(PathfindingFallback fallback = PathfindingFallback.DO_NOTHING)
    {
        if (PlayerInLineOfSight() || playerSighted > 0)
        {
            //print($"Player In Line: {PlayerInLineOfSight()}, Duration: {playerSighted}");
            direction = pathfinding.GetNextMove();
        }
        else if (fallback == PathfindingFallback.RANDOM_MOVEMENT)
        {
            direction = GetRandomDirection();
        }
        else if (fallback == PathfindingFallback.PATROL)
        {
            direction = GetWeightedDirection();
        }
        else if (fallback == PathfindingFallback.FOLLOW_WAYPOINTS)
        {
            if (waypoints.Length <= 1)
            {
                Debug.LogWarning("Too few waypoints established for " + name + ".");
                SetDirectionFromPathfinding(PathfindingFallback.PATROL);
            }
            else
            {
                direction = GetDirectionToNextWaypoint();
            }
        }
    }

    protected bool PlayerInLineOfSight()
    {
        RaycastHit2D hit = GetPlayerRaycast();
        if (hit.collider != null && hit.collider.name == "Player")
        {
            playerSighted = 4;
            return true;
        }
        else
        {
            playerSighted = Mathf.Max(0, playerSighted - 1);
            return false;
        }
    }

    protected RaycastHit2D GetPlayerRaycast()
    {
        float unitDistance = Vector2.Distance(
            player.currentTile,
            new Vector2(transform.position.x, transform.position.y));
        Vector2 raydirection =
            player.currentTile -
            new Vector2(transform.position.x, transform.position.y);

        raydirection.x /= unitDistance;
        raydirection.y /= unitDistance;

        float grossDistance = 3f;
        if (playerTempo.getStealth() == 8)
        {
            grossDistance = 2f;
        }
        else if (playerTempo.getStealth() <= 2)
        {
            grossDistance = 4f;
        }

        float distance = Mathf.Min(grossDistance, unitDistance);
        Debug.DrawRay(transform.position, raydirection * distance, Color.green, 1);
        return Physics2D.Raycast(transform.position, raydirection, distance, layerMask);
    }

    protected Vector2 GetRandomDirection()
    {
        float rand = Random.Range(0, 4);

        return rand switch
        {
            0 => Vector2.left,
            1 => Vector2.right,
            2 => Vector2.up,
            3 => Vector2.down,
            _ => Vector2.zero,
        };
    }

    private Vector2 GetWeightedDirection()
    {
        // n spots away: 1/(2^n) chance to go in random direction; otherwise go towards startingPoint
        float distance = Mathf.Round(Vector2.Distance(transform.position, startingPoint));
        float weight = 1f / Mathf.Pow(2, distance);

        if (Random.Range(0f, 1f) <= weight)
        {
            return GetRandomDirection();
        }
        else
        {
            return GetDirectionToStart();
        }
    }

    private Vector2 GetDirectionToStart()
    {
        float x = transform.position.x - startingPoint.x;
        float y = transform.position.y - startingPoint.y;
        float dx = Mathf.Abs(x);
        float dy = Mathf.Abs(y);

        if (dx > dy)
        {
            return x < 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            return y < 0 ? Vector2.up : Vector2.down;
        }
    }

    private Vector2 GetDirectionToNextWaypoint()
    {
        Vector2 next = waypoints[waypointIndex].position;
        if (next == currentTile)
        {
            Increment(ref waypointIndex, waypoints.Length - 1);
            next = waypoints[waypointIndex].position;
        }

        return pathfinding.GetNextMove(next);
    }

    /// <summary>
    /// Returns the direction to the player when the player is in the given line and not blocked by walls, otherwise returns null
    /// </summary>
    /// <param name="distance">The max distance from the enemy the player can be and still be detected</param>
    /// <param name="spread">The distance perpendicular to the line the player can be and still be detected</param>
    protected Vector2? PlayerIsInLine(int distance, int spread)
    {
        if (!PlayerInLineOfSight()) return null;

        Vector2 v = GetDistanceFromPlayer();

        float x = Mathf.Abs(v.x);
        float y = Mathf.Abs(v.y);
        float line = Mathf.Max(x, y);
        float offset = Mathf.Min(x, y);

        if (line <= distance && offset <= spread)
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
        if (direction == Vector2.zero) return Vector2.zero;

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
            if (!isMoving && player.acc != Accuracy.FAIL)
            {
                TakeDamage(1);
                player.CancelMoveCollide();
            }
            else
            {
                player.TakeDamage(1);
                player.CancelMoveCollide();
                if (isMoving)
                    ChainCancel(new Vector2Int(Mathf.CeilToInt(getNextPrime().x), Mathf.CeilToInt(getNextPrime().y)));
            }
            if (Health <= 0)
            {
                DestroyEnemy();
            }
            if (player.Health <= 0)
            {
                StartCoroutine(DeathScreen());
            }
        }

        base.OnCollisionEnter2D(collision);
    }

    void ChainCancel(Vector2Int v)
    {
        Debug.Log("One " + v);

        Vector2Int chain = new Vector2Int(
            Mathf.CeilToInt(enemyMap[v].currentTile.x),
            Mathf.CeilToInt(enemyMap[v].currentTile.y));

        Debug.Log("Chain" + enemyMap[v] + " " + enemyMap[v].currentTile);

        Enemy tempEnemy = enemyMap[v];
        tempEnemy.CancelMoveCollide();
        enemyMap.Remove(v);

        if (enemyMap.ContainsKey(chain))
        {
            ChainCancel(chain);
            if (!enemyMap.ContainsKey(chain))
                enemyMap.Add(chain, tempEnemy);
        }
        else
        {
            enemyMap.Add(chain, tempEnemy);
        }
    }

    protected override void RemoveFromMap()
    {
        enemyMap.Remove(
            new Vector2Int(
                Mathf.CeilToInt(getNextPrime().x),
                Mathf.CeilToInt(getNextPrime().y)));
    }

    protected override void RemoveFromMapPrime() //Yes I know the names are swapped don't touch it
    {
        enemyMap.Remove(
            new Vector2Int(
                Mathf.CeilToInt(getNext().x),
                Mathf.CeilToInt(getNext().y)));
    }

    private void DestroyEnemy()
    {
        GameObject death = Instantiate(deathAnimation, new Vector2(currentTile.x, currentTile.y), Quaternion.identity) as GameObject;

        Destroy(gameObject);
    }

    private IEnumerator DeathScreen(){
        gameState.SetPaused(true);
        gameState.SetFreeze(true);

        GameObject temp = Instantiate(curtain, new Vector2(currentTile.x, currentTile.y), Quaternion.identity) as GameObject;
        spriteRenderer.sortingOrder = 102;
        player.death();

        yield return new WaitForSeconds(2f);

        spriteRenderer.sortingOrder = 10;
        player.death2();

        GameObject death = Instantiate(deathAnimation, new Vector2(player.currentTile.x, player.currentTile.y), Quaternion.identity) as GameObject;
        death.GetComponent<SpriteRenderer>().sortingOrder = 102;

        yield return new WaitForSeconds(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Death");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

public enum PathfindingFallback
{
    DO_NOTHING,
    RANDOM_MOVEMENT,
    FOLLOW_WAYPOINTS,
    PATROL
}
