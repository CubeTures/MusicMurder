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
    
    Image image;
    Color a = Color.black, b = Color.white;

    public delegate void MetronomeBeat(float timestamp, float nextBeatTimestamp);
    MetronomeBeat onMetronomeBeat;

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
        image = GetComponent<Image>();
        StartCoroutine(Pulse());
        metro = GetComponent<AudioSource>();
    }

    IEnumerator Pulse()
    {
        ChangeDisplay();
        NotifyOnMetronomeBeat();
        yield return new WaitForSecondsRealtime(Interval);
        metro.Play();
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
        float nextBeatTimestamp = Time.time + (Interval);

        foreach (MetronomeBeat m in onMetronomeBeat.GetInvocationList())
        {
            m.Invoke(timestamp, nextBeatTimestamp);
        }

    }
}
