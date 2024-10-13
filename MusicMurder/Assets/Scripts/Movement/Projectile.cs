using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Movement
{
    Metronome metronome;

    new void Start()
    {
        base.Start();
        metronome = Metronome.Instance;
        SetListenStatus(true);
    }
    
    void Move(float timestamp, float nextBeatTimestamp)
    {
        direction = transform.right;
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
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
                metronome.ListenOnMetronomeBeat(Move);
            }
            else
            {
                metronome.UnlistenOnMetronomeBeat(Move);
            }
        }
    }
}
