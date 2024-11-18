using TMPro;
using UnityEngine;

public class FPS : OnMetronome
{
    int frameCount = 0;
    float dt = 0.0f;
    float fps = 0.0f;
    const int maxRate = 120;
    int beats = 0;
    const int changeOnBeats = 2;

    [SerializeField] TMP_Text fpsText;

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        base.OnMetronomeBeat(timestamp, failTimestamp, nextBeatTimestamp, startup);

        if (++beats == changeOnBeats)
        {
            CalculateFPS();
            UpdateDisplay();
            beats = 0;
        }
    }

    new void Update()
    {
        base.Update();
        frameCount++;
        dt += Time.deltaTime;
    }

    void CalculateFPS()
    {
        fps = frameCount / dt;
        frameCount = 0;
        dt = 0;
    }

    void UpdateDisplay()
    {
        int rate = Mathf.Min(Mathf.RoundToInt(fps), maxRate);
        fpsText.text = $"FPS {rate}";
    }
}
