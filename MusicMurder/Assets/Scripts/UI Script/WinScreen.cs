using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class WinScreen : MonoBehaviour
{
    TMPro.TextMeshProUGUI text;

    void Start()
    {
        text = GameObject.Find("Text").GetComponentInChildren<TMPro.TextMeshProUGUI>();
        StartCoroutine(rickRoll());
    }

    IEnumerator rickRoll(){
        yield return new WaitForSeconds(1.0f);

        Application.OpenURL("https://www.youtube.com/watch?v=xvFZjo5PgG0");
    }

    void ButtonClick(){
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        float endTime = Time.time+1;
        while(Time.time <= endTime){
            text.color = Color.Lerp(Color.clear, Color.white, endTime-Time.time);

            yield return null;
        }

        text.color = Color.clear;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Grid Evan");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
