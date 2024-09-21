using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{
    /* How to code player listener for metronome:
     * 0. Understand observer design pattern
     * 1. Create a new script and have it subscribe to this one
     * 2. Create an observer for the player and clicking to move
     * 3. Subscribe the new script to that new observer on the player
     * 4. Add rewards for when the player moves close to when the beat occurs
     */

    float bpm = 100;
    Image image;
    Color a = Color.red, b = Color.white;

    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(Pulse());
    }

    IEnumerator Pulse()
    {
        ChangeDisplay();
        yield return new WaitForSecondsRealtime(60 / bpm);
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
}
