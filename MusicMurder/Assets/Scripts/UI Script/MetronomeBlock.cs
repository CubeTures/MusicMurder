using UnityEngine;
using UnityEngine.UI;

public class MetronomeBlock : OnMetronome
{
    Image sr;
    [SerializeField] Color a, b;
    bool isA = true;

    private new void Start()
    {
        sr = GetComponent<Image>();
        base.Start();
    }

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        sr.color = isA ? b : a;
        isA = !isA;
        base.OnMetronomeBeat(timestamp, failTimestamp, nextBeatTimestamp, startup);
    }
}
