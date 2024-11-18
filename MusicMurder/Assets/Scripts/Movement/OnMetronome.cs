using UnityEngine;

public abstract class OnMetronome : MonoBehaviour
{
    protected Metronome metronome;
    protected GameState gameState;
    protected bool canAct = false;
    protected bool paused;

    protected void Start()
    {
        metronome = Metronome.Instance;
        gameState = GameState.Instance;
        SetListenStatus(true);
    }

    protected virtual void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        canAct = !startup && !paused;
        paused = gameState.Paused;
    }

    protected void Update()
    {
        paused = gameState.Paused || paused;

        if (paused)
        {
            canAct = false;
        }
    }

    protected void OnEnable()
    {
        SetListenStatus(true);
    }

    protected void OnDisable()
    {
        SetListenStatus(false);
    }

    protected void OnDestroy()
    {
        SetListenStatus(false);
    }

    void SetListenStatus(bool status)
    {
        if (metronome != null)
        {
            if (status)
            {
                metronome.ListenOnMetronomeBeat(OnMetronomeBeat);
            }
            else
            {
                metronome.UnlistenOnMetronomeBeat(OnMetronomeBeat);
            }
        }
    }
}
