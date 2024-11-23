using UnityEngine;

public class HealthUIScript : MonoBehaviour
{
    
    //IGNORE THE NAMING SCEME
    public GameObject Full;
    public GameObject Five;
    public GameObject Half;
    public GameObject three;
    public GameObject Low;
    public GameObject One;


    private void Start()
    {
        UpdateHealth();
    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void UpdateHealth(int health = 6)
    {
        //Not a great solution but gets the job done
        if (health >= 6)
        {
            Full.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Five.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Half.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            three.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Low.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            One.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        }

        if (health == 5)
        {
            Full.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Five.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Half.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            three.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Low.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            One.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        }

        if (health == 4)
        {
            Full.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Five.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Half.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            three.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Low.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            One.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        }

        if (health == 3)
        {
            Full.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Five.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Half.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            three.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Low.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            One.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            return;
        }
        else if (health == 2)
        {
            Full.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Five.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Half.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            three.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Low.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            One.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            return;
        }
        else if (health == 1)
        {
            Full.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Five.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Half.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            three.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            Low.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            One.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            return;
        }
        return;
    }
}
