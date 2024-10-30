using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnMetronome : MonoBehaviour
{
    Metronome metronome;

    protected void Start()
    {
        metronome = Metronome.Instance;
        SetListenStatus(true);
    }

    protected abstract void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup);

    private void OnEnable()
    {
        SetListenStatus(true);
    }

    private void OnDisable()
    {
        SetListenStatus(false);
    }

    private void OnDestroy()
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
