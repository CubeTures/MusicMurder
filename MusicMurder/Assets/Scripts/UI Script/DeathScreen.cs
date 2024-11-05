using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    Image metronome;
    TMPro.TextMeshProUGUI text;

    void Awake(){
        metronome = GameObject.Find("Metronome").GetComponent<Image>();
        text = GameObject.Find("Text").GetComponentInChildren<TMPro.TextMeshProUGUI>();
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
        float endTime = Time.time+1;
        while(Time.time <= endTime){
            metronome.color = Color.Lerp(Color.clear, Color.white, endTime-Time.time);
            text.color = Color.Lerp(Color.clear, Color.white, endTime-Time.time);

            yield return null;
        }

        metronome.color = Color.clear;
        text.color = Color.clear;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Grid Evan");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
