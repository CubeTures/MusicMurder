using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTile : OnMetronome
{
    protected int timerToDamage = 1;
    protected float fail;
    PlayerMovement player;
    bool playerInside = false;
    const string playerTag = "Player";

    new void Start()
    {
        player = PlayerMovement.Instance;
        base.Start();
    }

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        timerToDamage--;
        fail = timestamp -  failTimestamp;
        if(timerToDamage <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        // animate
        StartCoroutine(CheckDamage());
    }

    IEnumerator CheckDamage()
    {
        yield return new WaitForSeconds(fail);

        if (playerInside)
        {
            player.TakeDamage(1);
        }

        // make sure player never takes double damage
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(playerTag))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(playerTag))
        {
            playerInside = false;
        }
    }
}
