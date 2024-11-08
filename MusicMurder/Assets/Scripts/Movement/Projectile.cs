using UnityEngine;

public class Projectile : Movement
{
    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        direction = transform.right;
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
            pm.TakeDamage(1);
        }
        Destroy(gameObject);
    }
}
