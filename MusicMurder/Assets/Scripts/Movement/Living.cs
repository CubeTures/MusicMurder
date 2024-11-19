using System.Collections;
using UnityEngine;

public abstract class Living : Movement
{
    public int Health { get; protected set; }

    SpriteRenderer sr;
    GameObject particles;
    public HealthUIScript healthUI;

    const float flashDuration = .3f;
    readonly Color flashColor = Color.red;
    protected Color initialColor = Color.white;

    protected new void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        particles = Resources.Load<GameObject>("DamageParticles");

        //Gets the health UI so can update when player gets hurt (May be a better way to do this, pls don't judge me)
        healthUI = GameObject.FindGameObjectWithTag("HealthUI").GetComponent<HealthUIScript>();


        base.Start();
    }

    public bool TakeDamage(int damage)
    {
        Health -= damage;

        Hurt();

        return Health <= 0;
    }
    private void Hurt()
    {
        StopAllCoroutines();
        Instantiate(particles, transform.position, Quaternion.identity, transform);
        healthUI.UpdateHealth(Health);
        StartCoroutine(FlashRed());
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
