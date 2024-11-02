using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetronomeDisplay : OnMetronome
{
    [SerializeField] Transform left, right, center;
    [SerializeField] GameObject bar;

    float interpolateTime;
    float failDelay;
    float startupBeats = Metronome.STARTUP_BEATS;

    GameState state;
    PlayerTempo tempo;

    Dictionary<int, bool> complete = new();
    Queue<int> queue = new();
    Accuracy recentAccuracy;

    protected new void Start()
    {
        state = GameState.Instance;
        tempo = PlayerTempo.Instance;
        SetListenStatus(true);

        base.Start();
    }

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        interpolateTime = (nextBeatTimestamp - timestamp) * startupBeats;
        failDelay = timestamp - failTimestamp;

        if(!state.Paused)
        {
            StartCoroutine(MoveBar());
        }
    }   

    private void OnPlayerAccuracy(Accuracy accuracy)
    {
        int hash = queue.Dequeue();
        complete[hash] = true;
        recentAccuracy = accuracy;
    }

    IEnumerator MoveBar()
    {
        float t = interpolateTime;
        float mult = 1 / t;
        Accuracy accuracy = Accuracy.FAIL;
           
        Transform bl = Instantiate(bar, left.position, Quaternion.identity, transform).transform;
        Transform br = Instantiate(bar, right.position, Quaternion.identity, transform).transform;
        int hash = bl.GetHashCode();
        complete[hash] = false;
        queue.Enqueue(hash);

        while(t > 0)
        {
            t -= Time.deltaTime;
            Interpolate(t, mult, bl, left);
            Interpolate(t, mult, br, right);

            if (IsCompleted(hash))
            {
                accuracy = recentAccuracy;
                goto Completed;
            }

            yield return null;
        }

        t = failDelay;

        while(t > 0)
        {
            t -= Time.deltaTime;
            if(IsCompleted(hash))
            {
                accuracy = recentAccuracy;
                goto Completed;
            }

            yield return null;
        }

        
        Completed:

            BarCompleteEffect(accuracy, bl.position, br.position);
            Destroy(bl.gameObject);
            Destroy(br.gameObject);
            complete.Remove(hash);
            DisplayAccuracy(accuracy);
    }

    // variables as incoherent as possible -- just the way i like them
    void Interpolate(float t, float m, Transform n, Transform s)
    {
        n.position = Vector2.Lerp(center.position, s.position, t * m);
    }

    bool IsCompleted(int hash)
    {
        return complete[hash];
    }

    void BarCompleteEffect(Accuracy accuracy, Vector2 l, Vector2 r)
    {
        // some effect when destroying the bars
    }

    void DisplayAccuracy(Accuracy accuracy)
    {
        // display the text corresponding, plus any other effects
    }

    protected new void OnEnable()
    {
        SetListenStatus(true);
        base.OnEnable();
    }

    protected new void OnDisable()
    {
        SetListenStatus(false);
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
