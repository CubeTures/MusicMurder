using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Movement
{
    Metronome metronome;
    const string playerTag = "Player";
    protected int beatsBetweenActions = 0;
    int beatsSinceAction = 0;

    protected new void Start()
    {
        metronome = Metronome.Instance;
        SetListenStatus(true);
        base.Start();
    }

    void BaseMove(float timestamp, float nextBeatTimestamp)
    {
        Increment(ref beatsSinceAction, beatsBetweenActions);

        if(beatsSinceAction == beatsBetweenActions)
        {
            Move();
        }
    }

    protected abstract void Move();

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
            DestroyEnemy();
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
