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

    GameState gameState;
    public static readonly int STARTUP_BEATS = 4;
    int currentStartupBeats = 0;
    bool previouslyPaused = false;

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
    }

    private void Update()
    {
        if (!musicStarted)
        {
            music.Play();
            musicStarted = true;
        }

        float sampledTime = music.timeSamples / (music.clip.frequency * Interval);
        if (NewInterval(sampledTime))
        {
            Pulse();
        }
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
        print("Beat");
        if (!gameState.Freeze)
        {
            NotifyOnMetronomeBeat();
        }

        if (previouslyPaused && !gameState.Paused)
        {
            currentStartupBeats = STARTUP_BEATS;
        }

        previouslyPaused = gameState.Paused;
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

        Enemy.enemyMap.Clear();

        if (onMetronomeBeat != null)
        {
            foreach (MetronomeBeat m in onMetronomeBeat.GetInvocationList())
            {
                bool startup = currentStartupBeats-- > 0;
                currentStartupBeats = Mathf.Max(0, currentStartupBeats);

                m.Invoke(timestamp, failTimestamp, nextBeatTimestamp, startup);
            }
        }
    }
}
