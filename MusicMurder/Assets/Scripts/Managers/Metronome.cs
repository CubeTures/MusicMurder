using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{

    public static Metronome Instance { get; private set; }

    public const float SECONDS_PER_MINUTE = 60;
    public float BPM { get; private set; } = 120;
    public float Interval { get; private set; } // time between beats
    PlayerTempo tempo;
    AudioSource music;

    public delegate void MetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup);
    MetronomeBeat onMetronomeBeat;
    bool musicStarted = false;
    int previousInterval = 0;
    Queue<float> calculatedBeats = new();
    float offset;

    GameState gameState;
    public static readonly int STARTUP_BEATS = 4;
    public int currentStartupBeats = 0;
    bool paused = false;

    private void Awake()
    {
        if (Instance == null)
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
        music = GetComponent<AudioSource>();
        tempo = PlayerTempo.Instance;
        currentStartupBeats = STARTUP_BEATS;
        offset = PlayerPrefs.GetFloat("calibration");
    }

    private void Update()
    {
        if (!musicStarted && Time.time > float.Epsilon)
        {
            music.Play();
            musicStarted = true;
        }

        paused = gameState.Paused || paused;
        float sampledTime = music.timeSamples / (music.clip.frequency * Interval);
        if (NewInterval(sampledTime))
        {
            Pulse();
        }
    }

    float GetCalculatedBeat()
    {
        if (calculatedBeats.Count == 0)
        {
            calculatedBeats.Enqueue(Time.time + offset);
        }

        float time = calculatedBeats.Dequeue();
        calculatedBeats.Enqueue(time + Interval);
        return time;
    }

    float PeekNextCalculatedBeat()
    {
        return calculatedBeats.Peek();
    }

    bool NewInterval(float interval)
    {
        int floor = Mathf.FloorToInt(interval);
        if (floor != previousInterval)
        {
            previousInterval = floor;
            return true;
        }

        return false;
    }

    void Pulse()
    {
        if (paused)
        {
            currentStartupBeats = STARTUP_BEATS;
        }

        NotifyOnMetronomeBeat();

        paused = gameState.Paused;
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
        float timestamp = GetCalculatedBeat();
        float failTimestamp = timestamp + tempo.passInterval;
        float nextBeatTimestamp = PeekNextCalculatedBeat();

        //print($"Current: {Time.time}, timestamp: {timestamp}, fail: {failTimestamp}, next: {nextBeatTimestamp}, music: {music.time}");

        Enemy.enemyMap.Clear();

        if (onMetronomeBeat != null && !paused && !gameState.Freeze)
        {
            bool startup = currentStartupBeats-- > 0;
            currentStartupBeats = Mathf.Max(0, currentStartupBeats);

            foreach (MetronomeBeat m in onMetronomeBeat.GetInvocationList())
            {
                m.Invoke(timestamp, failTimestamp, nextBeatTimestamp, startup);
            }
        }
    }
}
