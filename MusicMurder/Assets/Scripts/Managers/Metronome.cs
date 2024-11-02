using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{

    public static Metronome Instance { get; private set; }

    public const float SECONDS_PER_MINUTE = 60;
    public float BPM { get; private set; } = 100;
    public float Interval {  get; private set; } // time between beats
    AudioSource metro;
    PlayerTempo tempo;
    
    Image image;
    Color a = Color.black, b = Color.white;

    public delegate void MetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup);
    MetronomeBeat onMetronomeBeat;

    GameState gameState;
    public static readonly int STARTUP_BEATS = 4;
    int currentStartupBeats = 0;
    bool previouslyPaused = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            SetInterval();
        } 
        else
        {
            Debug.LogError("Metronome Instance not Null");
        }
    }

    void SetInterval()
    {
        Interval = SECONDS_PER_MINUTE / BPM;
    }

    void Start()
    {
        gameState = GameState.Instance;
        image = GetComponent<Image>();
        metro = GetComponent<AudioSource>();
        tempo = PlayerTempo.Instance;
        StartCoroutine(Pulse());
    }

    IEnumerator Pulse()
    {
        NotifyOnMetronomeBeat();
        ChangeDisplay();
        metro.Play();

        if (previouslyPaused && !gameState.Paused)
        {
            currentStartupBeats = STARTUP_BEATS;
        }
        previouslyPaused = gameState.Paused;
        
        yield return new WaitForSecondsRealtime(Interval);
        StartCoroutine(Pulse());
    }

    void ChangeDisplay()
    {
        if(image.color == a)
        {
            image.color = b;
        } else
        {
            image.color = a;
        }
    }

    public void ListenOnMetronomeBeat(MetronomeBeat m)
    {
        onMetronomeBeat += m;
    }

    public void UnlistenOnMetronomeBeat(MetronomeBeat m)
    {
        onMetronomeBeat -= m;
    }

    private void NotifyOnMetronomeBeat()
    {
        float timestamp = Time.time;
        float failTimestamp = timestamp + tempo.passInterval;
        float nextBeatTimestamp = timestamp + (Interval);

        foreach (MetronomeBeat m in onMetronomeBeat.GetInvocationList())
        {
            bool startup = currentStartupBeats-- > 0;
            currentStartupBeats = Mathf.Max(0, currentStartupBeats);

            m.Invoke(timestamp, failTimestamp, nextBeatTimestamp, startup);
        }        
    }
}
