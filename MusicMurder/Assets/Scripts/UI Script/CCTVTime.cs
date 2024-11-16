using TMPro;
using UnityEngine;

public class CCTVTime : OnMetronome
{
    TMP_Text text;
    int beats = 0;
    const int changeOnBeats = 2;

    private new void Start()
    {
        text = GetComponent<TMP_Text>();
        base.Start();
    }

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        base.OnMetronomeBeat(timestamp, failTimestamp, nextBeatTimestamp, startup);

        if (beats == changeOnBeats)
        {
            text.text = GetTime();
            beats = 0;
        }

        beats++;
    }

    string GetTime()
    {
        int totalSeconds = Mathf.FloorToInt(Time.time);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int remainingSeconds = totalSeconds % 60;

        return $"{hours:D2}:{minutes:D2}:{remainingSeconds:D2}";
    }
}
