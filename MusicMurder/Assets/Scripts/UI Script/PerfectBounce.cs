using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;


public class PerfectBounce : MonoBehaviour
{


    Animation BounceAnimation;
    Animation Fadeout;

    public GameObject Perfect;
    public GameObject Pass;
    public GameObject Fail;
    GameObject [] BeatparticlesArray;
    ParticleSystem Beatparticles;


    public string activeBeatUI;
    public int disapearTime;
    public int timeint;

    // Start is called before the first frame update
    void Start()
    {
        Perfect = gameObject.transform.GetChild(0).gameObject;
        Pass = gameObject.transform.GetChild(1).gameObject;
        Fail = gameObject.transform.GetChild(2).gameObject;
        BounceAnimation = GetComponent<Animation>();
        timeint = 1;
        BeatparticlesArray = GameObject.FindGameObjectsWithTag("BeatEffects");
        Beatparticles = BeatparticlesArray[0].GetComponent<ParticleSystem>();
        
       
    }

    public void setPerfect()
    {
        Perfect.transform.localScale = new Vector3(1, 1, 1);
        Pass.transform.localScale = new Vector3(0, 0, 0);
        Fail.transform.localScale = new Vector3(0, 0, 0);
        activeBeatUI = "Perfect";
        timeint = 1;
        //Debug.Log("Perfect");
        Beatparticles.Play();

        return;
    }

    public void setPass()
    {
        Perfect.transform.localScale = new Vector3(0, 0, 0);
        Pass.transform.localScale = new Vector3(1, 1, 1);
        Fail.transform.localScale = new Vector3(0, 0, 0);
        activeBeatUI = "Pass";
        timeint = 1;
        //Debug.Log("pass");
        return;
    }
    public void setFail()
    {
        Perfect.transform.localScale = new Vector3(0, 0, 0);
        Pass.transform.localScale = new Vector3(0, 0, 0);
        Fail.transform.localScale = new Vector3(1, 1, 1);
        activeBeatUI = "Fail";
        timeint = 1;
        //Debug.Log("Fail");
        return;
    }

    public void PlayBounce()
    {
        BounceAnimation.Play("TestAnimation");
        //Debug.Log("BOUNCE");
        
        if (timeint == 0)
        {
            Perfect.transform.localScale = new Vector3(0, 0, 0);
            Pass.transform.localScale = new Vector3(0, 0, 0);
            Fail.transform.localScale = new Vector3(0, 0, 0);
        }
        timeint -= 1;
        return;
    }


    public void HideBeat()
    {
        return;
    }
}
