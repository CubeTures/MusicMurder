using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public abstract class Living : Movement
{
    public int Health { get; protected set; }

    SpriteRenderer sr;
    GameObject particles;
    const float flashDuration = .3f;
    readonly Color flashColor = Color.red;

    protected new void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        particles = Resources.Load<GameObject>("DamageParticles");

        base.Start();
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Hurt();
    }
    private void Hurt()
    {
        StopAllCoroutines();
        Instantiate(particles, transform.position, Quaternion.identity, transform);
        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        float t = flashDuration;
        float mult = 1 / t;
        Color initial = sr.color;

        while (t > 0)
        {
            sr.color = Color.Lerp(initial, flashColor, t * mult);
            t -= Time.deltaTime;
            yield return null;
        }
    }
}
