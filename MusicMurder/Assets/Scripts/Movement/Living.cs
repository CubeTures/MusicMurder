using System.Collections;
using UnityEngine;

public abstract class Living : Movement
{
    public int Health { get; protected set; }

    SpriteRenderer sr;
    GameObject particles;

    const float flashDuration = .3f;
    readonly Color flashColor = Color.red;
    protected Color initialColor = Color.white;

    Coroutine hurtRoutine;

    protected new void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        particles = Resources.Load<GameObject>("DamageParticles");

        base.Start();
    }

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        base.OnMetronomeBeat(timestamp, failTimestamp, nextBeatTimestamp, startup);
        //float midtime = (((timestamp + nextBeatTimestamp) / 2) + failTimestamp) / 2;
        //StartCoroutine(ResetIFrames(midtime));
    }

    IEnumerator ResetIFrames(float failTimestamp)
    {
        yield return new WaitForSeconds(failTimestamp - Time.time);

        if (name == "Player")
        {
            print("Reset iFrames");
        }
    }

    public bool TakeDamage(int damage)
    {
        Health -= damage;

        Hurt();

        return Health <= 0;
    }

    private void Hurt()
    {
        if (hurtRoutine != null)
        {
            StopCoroutine(hurtRoutine);
            hurtRoutine = null;
        }

        Instantiate(particles, transform.position, Quaternion.identity, transform);
        hurtRoutine = StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        float t = flashDuration;
        float mult = 1 / t;
        Color initial = initialColor;

        while (t > 0)
        {
            sr.color = Color.Lerp(initial, flashColor, t * mult);
            t -= Time.deltaTime;
            yield return null;
        }
    }
}
