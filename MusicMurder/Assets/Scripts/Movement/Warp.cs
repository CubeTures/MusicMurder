using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Warp : MonoBehaviour
{
    SpriteRenderer curtain;

    void Start(){
        curtain = GameObject.FindWithTag("Curtain").GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            StartCoroutine(DrawCurtain());
    }

    private IEnumerator DrawCurtain(){
        GameObject.FindWithTag("Canvas").SetActive(false);

        float endTime = Time.time + 1;
        while (Time.time <= endTime)
        {
            curtain.color = Color.Lerp(Color.black, Color.clear, endTime - Time.time);

            yield return null;
        }

        Scene scene = SceneManager.GetActiveScene();
        switch(scene.name){
            case "Tutorial":
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Storerooms-Grid 2");

                // Wait until the asynchronous scene fully loads
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
                break;
        }
    }
}
