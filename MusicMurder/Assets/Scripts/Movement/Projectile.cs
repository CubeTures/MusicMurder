using UnityEngine;

public class Projectile : Movement
{
    SpriteRenderer sr;
    PlayerTempo playerTempo;
    PlayerMovement player;
    HealthUIScript healthUI;

    protected new void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        playerTempo = PlayerTempo.Instance;
        player = PlayerMovement.Instance;
        healthUI = GameObject.FindGameObjectWithTag("HealthUI").GetComponent<HealthUIScript>();

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
            if (player.diz)
            {
                GameObject dizzy = GameObject.FindGameObjectWithTag("Dizzy");
                if (dizzy != null)
                {
                    Destroy(dizzy);
                }
                player.diz = false;
                playerTempo.dizzyCount = 0;
            }

            healthUI.UpdateHealth(player.Health);

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
