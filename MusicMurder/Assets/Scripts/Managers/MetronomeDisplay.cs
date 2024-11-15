using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class breaks if the tempo changes in the middle of action. 
/// We shouldn't need to do this, but it's something to keep in mind.
/// </summary>
public class MetronomeDisplay : OnMetronome
{
    [SerializeField] Transform left, right, center;
    [SerializeField] GameObject bar;

    float interpolateTime;
    float failDelay;
    float latestTimestamp;
    float timestampDifference;
    float startupBeats = Metronome.STARTUP_BEATS;
    bool prevPaused = false;

    PlayerTempo tempo;

    Dictionary<float, bool> complete = new();
    Queue<float> queue = new();
    Accuracy recentAccuracy;
    List<GameObject> activeBars = new();

    protected new void Start()
    {
        tempo = PlayerTempo.Instance;
        SetListenStatus(true);

        base.Start();
    }

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        base.OnMetronomeBeat(timestamp, failTimestamp, nextBeatTimestamp, startup);

        latestTimestamp = timestamp;
        timestampDifference = nextBeatTimestamp - timestamp;
        interpolateTime = timestampDifference * startupBeats;
        failDelay = timestamp - failTimestamp;
        float estimatedTimestamp = timestamp + timestampDifference * startupBeats;

        if (!gameState.Paused && !prevPaused)
        {
            StartCoroutine(MoveBar(estimatedTimestamp));
        }

        prevPaused = gameState.Paused;
    }

    private void OnPlayerAccuracy(Accuracy accuracy)
    {
        if (queue.Count == 0)
        {
            return;
        }

        float timestamp = queue.Peek();
        if (Mathf.Abs(timestamp - latestTimestamp) < timestampDifference)
        {
            queue.Dequeue();
            complete[timestamp] = true;
            recentAccuracy = accuracy;
        }
    }

    IEnumerator MoveBar(float endTimestamp)
    {
        float startTime = Time.time;
        float t = endTimestamp - startTime;
        float mult = 1 / t;
        Accuracy accuracy = Accuracy.FAIL;

        Transform bl = Instantiate(bar, left.position, Quaternion.identity, transform).transform;
        Transform br = Instantiate(bar, right.position, Quaternion.identity, transform).transform;
        bl.name = $"{endTimestamp} Left";
        br.name = $"{endTimestamp} Right";
        complete[endTimestamp] = false;
        queue.Enqueue(endTimestamp);
        activeBars.Add(bl.gameObject);
        activeBars.Add(br.gameObject);

        while (t > 0)
        {
            if (gameState.Paused)
            {
                Clear();
                goto Clear;
            }

            t = endTimestamp - Time.time;
            Interpolate(t, mult, bl, left);
            Interpolate(t, mult, br, right);

            if (IsCompleted(endTimestamp))
            {
                accuracy = recentAccuracy;
                goto Completed;
            }

            yield return null;
        }

        t = failDelay;

        while (t > 0)
        {
            if (gameState.Paused)
            {
                Clear();
                goto Clear;
            }

            t = failDelay + endTimestamp - Time.time;
            if (IsCompleted(endTimestamp))
            {
                accuracy = recentAccuracy;
                goto Completed;
            }

            yield return null;
        }


    Completed:

        BarCompleteEffect(accuracy, bl.position, br.position);
        DisplayAccuracy(accuracy);

    Clear:

        activeBars.Remove(bl.gameObject);
        activeBars.Remove(br.gameObject);
        Destroy(bl.gameObject);
        Destroy(br.gameObject);
        complete.Remove(endTimestamp);
    }

    // variables as incoherent as possible -- just the way i like them
    void Interpolate(float t, float m, Transform n, Transform s)
    {
        n.position = Vector2.Lerp(center.position, s.position, t * m);
    }

    bool IsCompleted(float timestamp)
    {
        return complete[timestamp];
    }

    void BarCompleteEffect(Accuracy accuracy, Vector2 l, Vector2 r)
    {
        // some effect when destroying the bars
    }

    void DisplayAccuracy(Accuracy accuracy)
    {
        // display the text corresponding, plus any other effects
    }

    void Clear()
    {
        queue.Clear();
    }

    void DestroyBars()
    {
        foreach (GameObject obj in activeBars)
        {
            Destroy(obj);
        }

        activeBars.Clear();
    }

    protected new void OnEnable()
    {
        SetListenStatus(true);
        base.OnEnable();
    }

    protected new void OnDisable()
    {
        SetListenStatus(false);
        Clear();
        DestroyBars();
        base.OnDisable();
    }

    protected new void OnDestroy()
    {
        SetListenStatus(false);
        base.OnDestroy();
    }

    void SetListenStatus(bool status)
    {
        if (tempo != null)
        {
            if (status)
            {
                tempo.ListenOnPlayerAccuracy(OnPlayerAccuracy);
            }
            else
            {
                tempo.UnlistenOnPlayerAccuracy(OnPlayerAccuracy);
            }
        }
    }
}
