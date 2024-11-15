using UnityEngine;
using UnityEngine.Events;

public abstract class Boss : Enemy
{
    int[] healths;
    int phase;

    [SerializeField] UnityEvent[] interphase2Events, interphase3Events;

    SpriteRenderer sr;

    protected new void Start()
    {

        Health = healths[0];
        boss = true;
        sr = GetComponent<SpriteRenderer>();

        base.Start();
    }

    protected override void Move()
    {
        if (phase == 0)
        {
            Phase1();
        }
        else if (phase == 1)
        {
            Phase2();
        }
        else if (phase == 2)
        {
            Phase3();
        }

        if (Health < 0)
        {
            if (phase == 2)
            {
                Death();
                return;
            }

            Interphase();

            phase++;
            Health = healths[phase];
        }
    }

    protected abstract void Phase1();
    protected abstract void Phase2();
    protected abstract void Phase3();

    void Interphase()
    {
        // zoom in on boss if time permits

        if (phase == 1)
        {
            initialColor = Color.yellow;
            sr.color = Color.yellow;
            InvokeInterphaseEvents(interphase2Events);
            Interphase2();
        }
        else if (phase == 2)
        {
            initialColor = Color.red;
            sr.color = Color.red;
            InvokeInterphaseEvents(interphase3Events);
            Interphase3();
        }
    }

    protected abstract void Interphase2();
    protected abstract void Interphase3();

    void InvokeInterphaseEvents(UnityEvent[] events)
    {
        foreach (UnityEvent @event in events)
        {
            @event.Invoke();
        }
    }

    void Death()
    {
        DestroyEnemy();
    }
}
