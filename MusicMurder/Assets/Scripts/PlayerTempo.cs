using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTempo : MonoBehaviour
{
    Metronome metronome;
    PlayerMovement player;

    float lastBeat = 0, nextBeat = 0;
    float perfectMargin, passMargin;
    float prevInterval = 0;
    const float perfectInterval = .1f;
    const float passInterval = .2f;

    int numActionsInInterval = 0;
    bool hasMovedSinceTempoChange = false;

    TMP_Text text;

    void Start()
    {
        metronome = Metronome.Instance;
        player = PlayerMovement.Instance;
        text = GetComponent<TMP_Text>();

        SetListenStatus(true);
        SetMargins();
    }

    void SetMargins()
    {
        if (metronome.Interval != prevInterval)
        {
            prevInterval = metronome.Interval;
            perfectMargin = prevInterval * perfectInterval;
            passMargin = prevInterval * passInterval;

            Debug.Log("Perfect Margin: " +  perfectMargin + ", Pass Margin: " + passMargin);
            hasMovedSinceTempoChange = false;
        }
    }

    void OnMetronomeBeat(float timestamp, float nextBeatTimestamp)
    {
        lastBeat = timestamp;
        nextBeat = nextBeatTimestamp;

        /* TODO: Punish player for standing still
         *      (with the leeway of being able to stand still to adjust to the new tempo,
         *      or on first spawn / respawn, etc.)
         * 
         * This code doesn't work, and here's why:
         *      The player can either press before or after the main beat
         *      I'm only keeping track of the number of times a player pressing in an interval
         *      Meaning, if the player matches one beat AFTER the beat, 
         *          but still succeeds, they are in the new interval
         *      However, if they miss the next beat, the "numActionsInInterval" var is > 0, 
         *          since they acted on the later half of the previous beat, meaning they don't get punished
         *      Fixing this bug will also fix the bug where they can spam while in the margin
         */

        /*if(hasMovedSinceTempoChange && numActionsInInterval == 0)
        {
            SetAccuracy(Accuracy.FAIL);
        }*/

        numActionsInInterval = 0;
    }

    void OnPlayerAction(PlayerActionType actionType, float timestamp)
    {
        numActionsInInterval++;
        hasMovedSinceTempoChange = true;

        float delatLast = Mathf.Abs(lastBeat - timestamp);
        float delatNext = Mathf.Abs(nextBeat - timestamp);
        float delta = Mathf.Min(delatLast, delatNext);

        // TODO: Fix accuracy so you can't spam while in margin
        if (delta < perfectMargin)
        {
            SetAccuracy(Accuracy.PERFECT);
        }
        else if (delta < passMargin)
        {
            SetAccuracy(Accuracy.PASS);
        }
        else if (numActionsInInterval > 1)
        {
            SetAccuracy(Accuracy.FAIL);
        }
    }

    void SetAccuracy(Accuracy acc)
    {
        string s = GetAccuracyString(acc);
        text.text = GetAccuracyString(acc);
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

enum Accuracy { 
    PERFECT,
    PASS,
    FAIL
}