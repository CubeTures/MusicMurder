using UnityEngine;

public class Projectile : Movement
{
    SpriteRenderer sr;

    protected new void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        base.Start();
    }

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        base.OnMetronomeBeat(timestamp, failTimestamp, nextBeatTimestamp, startup);

        if (!canAct) return;
        direction = transform.right;
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
            bool died = pm.TakeDamage(1);

            if (died)
            {
                sr.sortingOrder = 102;
                pm.Death();
                return;
            }
        }
        Destroy(gameObject);
    }
}
