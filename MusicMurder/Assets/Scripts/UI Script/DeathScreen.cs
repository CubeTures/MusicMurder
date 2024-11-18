using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public static string OriginScene { get; set; }

    AudioSource determination;
    AudioSource audioSource;
    Image metronome;
    TMPro.TextMeshProUGUI text;
    Camera _camera;

    [SerializeField] Sprite[] frames = new Sprite[13];

    void Awake()
    {
        metronome = GameObject.Find("Metronome").GetComponent<Image>();
        text = GameObject.Find("Text").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        determination = GameObject.Find("Canvas").GetComponent<AudioSource>();
        StartCoroutine(BreakMetronome());
        _camera = GetComponent<Camera>();
        float _1A = 0x1A / 255f;
        _camera.backgroundColor = new Color(_1A, _1A, _1A);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    IEnumerator LoadYourAsyncScene()
    {
        float endTime = Time.time + 1;
        while (Time.time <= endTime)
        {
            text.color = Color.Lerp(Color.clear, Color.red, endTime - Time.time);
            _camera.backgroundColor = Color.Lerp(Color.clear, new Color(.2f, .2f, .2f), endTime - Time.time);

            yield return null;
        }

        text.color = Color.clear;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(OriginScene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator BreakMetronome()
    {
        yield return new WaitForSeconds(.5f);
        audioSource.Play();
        yield return new WaitForSeconds(.5f);
        audioSource.Play();
        yield return new WaitForSeconds(.75f);
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        audioSource.Play();
        for (int i = 1; i < 7; i++)
        {
            metronome.sprite = frames[i];
            yield return new WaitForSeconds(.2f);
        }
        yield return new WaitForSeconds(.5f);
        metronome.GetComponent<AudioSource>().Play();
        for (int i = 7; i < 13; i++)
        {
            metronome.sprite = frames[i];
            yield return new WaitForSeconds(.1f);
        }
        metronome.sprite = null;
        metronome.color = Color.clear;

        yield return new WaitForSeconds(.5f);

        determination.Play();
        text.color = Color.red;
    }
}
