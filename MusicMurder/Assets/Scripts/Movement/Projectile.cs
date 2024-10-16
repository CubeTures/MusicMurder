using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Movement
{
    protected override void OnMetronomeBeat(float timestamp, float nextBeatTimestamp, bool startup)
    {
        direction = transform.right;
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
