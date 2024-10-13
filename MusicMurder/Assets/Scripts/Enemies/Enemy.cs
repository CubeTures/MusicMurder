using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Movement
{
    Metronome metronome;
    GameState gameState;
    const string playerTag = "Player";
    protected int beatsBetweenActions = 0;
    protected PlayerMovement player;
    protected Pathfinding pathfinding;
    int beatsSinceAction = 0;

    readonly int layerMask = ~(1 << 2);

    protected new void Start()
    {
        metronome = Metronome.Instance;
        gameState = GameState.Instance;
        player = PlayerMovement.Instance;

        SetListenStatus(true);
        base.Start();
        
        health = 1;
        pathfinding = new Pathfinding(transform);
    }

    void BaseMove(float timestamp, float nextBeatTimestamp)
    {
        if (gameState.Paused) return;

        Increment(ref beatsSinceAction, beatsBetweenActions);

        if(beatsSinceAction == beatsBetweenActions)
        {
            Move();
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
                health--;
                player.CancelMove();
            }else{
                player.TakeDamage(1);
                CancelMove();
                player.CancelMove();
            }
            if(health <= 0){
                DestroyEnemy();
            }
            if(player.GetHealth() <= 0){
                Debug.Log("Player died");
            }
        }

        base.OnCollisionEnter2D(collision);
    }

    void DestroyEnemy()
    {
        Debug.Log("Enemy Died");
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        SetListenStatus(true);
    }

    private void OnDisable()
    {
        SetListenStatus(false);
    }

    private void OnDestroy()
    {
        SetListenStatus(false);
    }

    void SetListenStatus(bool status)
    {
        if (metronome != null)
        {
            if (status)
            {
                metronome.ListenOnMetronomeBeat(BaseMove);
            }
            else
            {
                metronome.UnlistenOnMetronomeBeat(BaseMove);
            }
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
