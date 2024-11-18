using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calibration : OnMetronome
{
    Queue<float> offsets = new();
    float timestamp, nextBeatTimestamp;
    float loadedOffset;

    [SerializeField] TMP_Text averageText, deviationText;

    protected new void Start()
    {
        loadedOffset = PlayerPrefs.GetFloat("calibration");
        SetDisplays();
        base.Start();
    }

    private new void Update()
    {
        base.Update();
        if (KeyPressed())
        {
            AddOffset();
            SetDisplays();
        }
    }

    protected override void OnMetronomeBeat(float timestamp, float failTimestamp, float nextBeatTimestamp, bool startup)
    {
        this.timestamp = timestamp;
        this.nextBeatTimestamp = nextBeatTimestamp;
        base.OnMetronomeBeat(timestamp, failTimestamp, nextBeatTimestamp, startup);
    }

    void AddOffset()
    {
        float closestTimestamp = GetClosestTimestamp();
        float offset = closestTimestamp - Time.time;

        offsets.Enqueue(offset);
    }

    float GetClosestTimestamp()
    {
        float current = Mathf.Abs(timestamp - Time.time);
        float next = Mathf.Abs(nextBeatTimestamp - Time.time);

        return current < next ? timestamp : nextBeatTimestamp;
    }

    bool KeyPressed()
    {
        return Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.DownArrow);
    }

    void SetDisplays()
    {
        float mean = offsets.Count > 0 ? offsets.Sum() / offsets.Count : 0;
        float deviation = offsets.Count > 0 ? offsets.Aggregate(0f, (accumulator, item) =>
        {
            return accumulator + Mathf.Abs(item - mean);
        }) / offsets.Count : 0;

        float prettyMean = mean;
        float prettyDeviation = (1 - deviation) * 100;

        averageText.text = $"New Offset:\n{prettyMean:0.#######}\nLoaded Offset:\n{loadedOffset:0.#######}";
        deviationText.text = $"Consistency:\n{prettyDeviation:0}%\nSamples:\n{offsets.Count}";
        PrintQueue();
    }

    void PrintQueue()
    {
        string result = "[ ";
        foreach (float offset in offsets)
        {
            result += $"{offset:0.#####} ";
        }
        result += "]";

        print(result);
    }

    public void ClearQueue()
    {
        offsets.Clear();
        SetDisplays();
    }

    public void SaveOffset()
    {
        float mean = offsets.Sum() / offsets.Count;
        PlayerPrefs.SetFloat("calibration", mean);
        PlayerPrefs.Save();
    }

    public void ClearSavedOffset()
    {
        PlayerPrefs.DeleteKey("calibration");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
