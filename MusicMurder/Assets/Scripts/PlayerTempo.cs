using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Metronome;

public class PlayerTempo : MonoBehaviour
{
    public static PlayerTempo Instance { get; private set; }

    Metronome metronome;
    PlayerMovement player;

    float lastBeat = 0, nextBeat = 0;
    float perfectInterval, passInterval;
    float prevInterval = 0;
    const float perfectMargin = .1f;
    const float passMargin = .2f;

    bool movedSinceTempoChange = false;
    bool movedThisBeat = false, movedNextBeat = false;

    TMP_Text text;

    public delegate void PlayerAccuracy(Accuracy accuracy);
    PlayerAccuracy onPlayerAccuracy;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } 
        else
        {
            Debug.LogError("Player Tempo Instance no Null");
        }
    }

    void Start()
    {
        metronome = Metronome.Instance;
        player = PlayerMovement.Instance;
        text = GetComponent<TMP_Text>();

        SetListenStatus(true);
        SetIntervals();
    }

    void SetIntervals()
    {
        if (metronome.Interval != prevInterval)
        {
            prevInterval = metronome.Interval;
            perfectInterval = prevInterval * perfectMargin;
            passInterval = prevInterval * passMargin;
            movedSinceTempoChange = false;

            Debug.Log("Perfect Interval: " +  perfectInterval + "s, Pass Interval: " + passInterval + "s");
        }
    }

    void OnMetronomeBeat(float timestamp, float nextBeatTimestamp)
    {
        lastBeat = timestamp;
        nextBeat = nextBeatTimestamp;
        movedThisBeat = movedNextBeat;
        movedNextBeat = false;

        //StartCoroutine(PenalizeNoAction());
    }

    // IEnumerator PenalizeNoAction()
    // {
        // yield return new WaitForSeconds(passInterval);
        // if (movedSinceTempoChange && !movedThisBeat)
        // {
        //     SetAccuracy(Accuracy.FAIL);
        // }
    // }

    void OnPlayerAction(PlayerActionType actionType, float timestamp)
    {
        movedSinceTempoChange = true;

        float deltaLast = Mathf.Abs(lastBeat - timestamp);
        float deltaNext = Mathf.Abs(nextBeat - timestamp);
        float delta = Mathf.Min(deltaLast, deltaNext);
        bool thisBeat = deltaLast < deltaNext;

        /* Fail Conditions:
         *      User moves outside of the pass interval
         *      User moves more than once on beat (this beat)
         *      User moves more than once on beat (next beat)
         */

        if(delta >= passInterval)
        {
            SetAccuracy(Accuracy.FAIL);
        }
        else if(movedThisBeat && thisBeat)
        {
            SetAccuracy(Accuracy.FAIL);
        }
        else if(movedNextBeat && !thisBeat)
        {
            SetAccuracy(Accuracy.FAIL);
        }

        else if (delta < perfectInterval)
        {
            SetAccuracy(Accuracy.PERFECT);
        }
        else if (delta < passInterval)
        {
            SetAccuracy(Accuracy.PASS);
        }

        movedThisBeat = thisBeat || movedThisBeat;
        movedNextBeat = !thisBeat || movedNextBeat;
    }

    void SetAccuracy(Accuracy acc)
    {
        string s = GetAccuracyString(acc);
        text.text = GetAccuracyString(acc);
        NotifyOnPlayerAccuracy(acc);
    }

    string GetAccuracyString(Accuracy acc)
    {
        switch(acc)
        {
            case Accuracy.PERFECT:
                return "Perfect";
            case Accuracy.PASS:
                return "Pass";
            case Accuracy.FAIL:
                return "Fail";
            default:
                return "NULL";
        }
    }

    public void ListenOnPlayerAccuracy(PlayerAccuracy m)
    {
        onPlayerAccuracy += m;
    }

    public void UnlistenOnPlayerAccuracy(PlayerAccuracy m)
    {
        onPlayerAccuracy -= m;
    }

    private void NotifyOnPlayerAccuracy(Accuracy accuracy)
    {

        foreach (PlayerAccuracy m in onPlayerAccuracy.GetInvocationList())
        {
            m.Invoke(accuracy);
        }

    }

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
        if(metronome != null && player != null)
        {
            if (status)
            {
                metronome.ListenOnMetronomeBeat(OnMetronomeBeat);
                player.ListenOnPlayerAction(OnPlayerAction);
            }
            else
            {
                metronome.UnlistenOnMetronomeBeat(OnMetronomeBeat);
                player.UnlistenOnPlayerAction(OnPlayerAction);
            }
        }
    }
}

public enum Accuracy { 
    PERFECT,
    PASS,
    FAIL
}