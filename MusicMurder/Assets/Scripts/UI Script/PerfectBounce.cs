using UnityEngine;


public class PerfectBounce : MonoBehaviour
{

    public float timer = 0;
    public float bounceRate = 4;

    Animation BounceAnimation;

    public GameObject Perfect;
    public GameObject Pass;
    public GameObject Fail;

    public string activeBeatUI;

    // Start is called before the first frame update
    void Start()
    {
        Perfect = gameObject.transform.GetChild(0).gameObject;
        Pass = gameObject.transform.GetChild(1).gameObject;
        Fail = gameObject.transform.GetChild(2).gameObject;
        BounceAnimation = GetComponent<Animation>();
    }

    public void setPerfect()
    {
        Perfect.transform.localScale = new Vector3(1, 1, 1);
        Pass.transform.localScale = new Vector3(0, 0, 0);
        Fail.transform.localScale = new Vector3(0, 0, 0);
        activeBeatUI = "Perfect";
        //Debug.Log("Perfect");
        return;
    }

    public void setPass()
    {
        Perfect.transform.localScale = new Vector3(0, 0, 0);
        Pass.transform.localScale = new Vector3(1, 1, 1);
        Fail.transform.localScale = new Vector3(0, 0, 0);
        activeBeatUI = "Pass";
        //Debug.Log("pass");
        return;
    }
    public void setFail()
    {
        Perfect.transform.localScale = new Vector3(0, 0, 0);
        Pass.transform.localScale = new Vector3(0, 0, 0);
        Fail.transform.localScale = new Vector3(1, 1, 1);
        activeBeatUI = "Fail";
        //Debug.Log("Fail");
        return;
    }

    public void PlayBounce()
    {
        BounceAnimation.Play("TestAnimation");
        //Debug.Log("BOUNCE");
        return;
    }
}
