using System.Collections;
using UnityEngine;

public class DamageTile : OnMetronome
{
    protected int timerToDamage = 0;
    bool setup = false;
    DamageTileCreator creator;

    protected float fail;
    PlayerMovement player;
    bool playerInside = false;
    const string playerTag = "Player";
    [SerializeField] GameObject explosion;

    new void Start()
    {
        player = PlayerMovement.Instance;
        base.Start();

        if (IsInvalid())
        {
            Destroy(gameObject);
        }
    }

    bool IsInvalid()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(.5f, .5f), 0);
        return InsideWall(colliders);
    }

    bool InsideWall(Collider2D[] colliders)
    {
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Walls"))
            {
                return true;
            }
        }

        return false;
    }

    public void Setup(int timer)
    {
        timerToDamage = timer;
        setup = true;
    }

    public void Setup(int timer, DamageTileCreator creator)
    {
        Setup(timer);
        this.creator = creator;
    }

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        base.OnMetronomeBeat(timestamp, failTimestamp, nextBeatTimestamp, startup);

        if (!canAct) return;
        if (!setup)
        {
            Debug.LogWarning("Damage tile never setup");
        }

        timerToDamage--;
        fail = timestamp - failTimestamp;
        if (timerToDamage <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        // animate
        StartCoroutine(CheckDamage());
        GameObject exp = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
    }

    IEnumerator CheckDamage()
    {
        yield return new WaitForSeconds(fail);

        if (playerInside)
        {
            player.TakeDamage(1);
        }

        // make sure player never takes double damage
        if (creator != null) creator.TileDestroyed(this);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            playerInside = false;
        }
    }
}
