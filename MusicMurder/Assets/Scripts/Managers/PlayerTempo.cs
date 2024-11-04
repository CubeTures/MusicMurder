using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerTempo : MonoBehaviour
{
    public static PlayerTempo Instance { get; private set; }

    Metronome metronome;
    PlayerMovement player;

    [SerializeField] private int stealth = 8;

    float lastBeat = 0, nextBeat = 0;
    public float perfectInterval, passInterval;
    float prevInterval = 0;
    const float perfectMargin = .12f;
    const float passMargin = .24f;

    //bool movedSinceTempoChange = false;
    bool movedThisBeat = false, movedNextBeat = false;

    TMP_Text text;

    public delegate void PlayerAccuracy(Accuracy accuracy);
    PlayerAccuracy onPlayerAccuracy;

    //Not sure best way to code this but
    public PerfectBounce Perfect;

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

        
        

        //Gets the PerfectUiObject


        Perfect = GameObject.FindGameObjectWithTag("Perfect").GetComponent<PerfectBounce>();

        

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
            //movedSinceTempoChange = false;

            Debug.Log("Perfect Interval: " +  perfectInterval + "s, Pass Interval: " + passInterval + "s");
        }
    }

    void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
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
        //movedSinceTempoChange = true;

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
            stealth = Mathf.Max(0, stealth - 2);
        }
        else if(movedThisBeat && thisBeat)
        {
            SetAccuracy(Accuracy.FAIL);
            stealth = Mathf.Max(0, stealth - 2);
        }
        else if(movedNextBeat && !thisBeat)
        {
            SetAccuracy(Accuracy.FAIL);
            stealth = Mathf.Max(0, stealth - 2);
        }

        else if (delta < perfectInterval)
        {
            SetAccuracy(Accuracy.PERFECT);
            stealth = Mathf.Min(8, stealth + 2);
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
        bounceAnimation();
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

    public int getStealth(){
        return stealth;
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

    void bounceAnimation()
    {
        Perfect.PlayBounce();
        return;
    }
}




public enum Accuracy { 
    PERFECT,
    PASS,
    FAIL
}