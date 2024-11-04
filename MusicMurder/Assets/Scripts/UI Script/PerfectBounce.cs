using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;


public class PerfectBounce : MonoBehaviour
{

    public float timer = 0;
    public float bounceRate = 4;

    Animation BounceAnimation;

    // Start is called before the first frame update
    void Start()
    {
        BounceAnimation = GetComponent<Animation>();
    }

   
 
    public void PlayBounce()
    {
        BounceAnimation.Play("TestAnimation");
    }
}
