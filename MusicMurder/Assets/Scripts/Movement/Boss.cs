using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Boss : Enemy
{
    [SerializeField] UnityEvent interphase2Events, interphase3Events, onDeathEvents;

    protected int[] healths;
    int phase;

    SpriteRenderer spr;
    CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin shake;

    protected new void Start()
    {
        boss = true;
        spr = GetComponent<SpriteRenderer>();
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        shake = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        base.Start();

        Health = healths[0];
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
    }

    protected abstract void Phase1();
    protected abstract void Phase2();
    protected abstract void Phase3();

    IEnumerator Interphase()
    {
        gameState.SetPaused(true);

        virtualCamera.Priority = 15;

        if (phase == 1)
        {
            StartCoroutine(SlideColor(1, Color.yellow));
        }
        else if (phase == 2)
        {
            StartCoroutine(SlideColor(1, Color.red));
        }

        yield return new WaitForSeconds(1);

        if (phase == 1)
        {
            initialColor = Color.yellow;
            spr.color = Color.yellow;
            InvokeEvents(interphase2Events);
            Interphase2();
        }
        else if (phase == 2)
        {
            initialColor = Color.red;
            spr.color = Color.red;
            InvokeEvents(interphase3Events);
            Interphase3();
        }
        else
        {
            virtualCamera.transform.parent = transform.parent;
            Destroy(virtualCamera.gameObject, 10);
            Death();
        }

        virtualCamera.Priority = 0;

        gameState.SetPaused(false);
    }

    IEnumerator SlideColor(float duration, Color color)
    {
        float t = duration;
        float mult = 1 / t;

        while (t > 0)
        {
            t -= Time.deltaTime;
            spr.color = Color.Lerp(initialColor, color, 1 - (t * mult));
            yield return new WaitForEndOfFrame();
        }
    }

    protected abstract void Interphase2();
    protected abstract void Interphase3();

    protected override void DestroyEnemy()
    {
        phase++;
        if (phase != 3)
        {
            Health = healths[phase];
        }

        StartCoroutine(Interphase());
    }

    void Death()
    {
        OnDeath();
        InvokeEvents(onDeathEvents);
        base.DestroyEnemy();
    }

    protected abstract void OnDeath();

    void InvokeEvents(UnityEvent events)
    {
        events.Invoke();
    }
}
