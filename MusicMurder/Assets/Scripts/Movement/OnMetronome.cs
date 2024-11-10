using UnityEngine;

public abstract class OnMetronome : MonoBehaviour
{
    Metronome metronome;
    protected GameState gameState;
    protected bool canAct = false;
    private bool previouslyPaused;

    protected void Start()
    {
        metronome = Metronome.Instance;
        gameState = GameState.Instance;
        SetListenStatus(true);
    }

    protected virtual void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        previouslyPaused = gameState.Paused;

        canAct = !gameState.Paused && !startup && !previouslyPaused && !gameState.Freeze;
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
