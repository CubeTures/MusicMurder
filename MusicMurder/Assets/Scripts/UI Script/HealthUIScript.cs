using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.ShaderData;

public class HealthUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Full;
    public GameObject Half;
    public GameObject Low;

    


    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateHealth(int health)
    {

        if (health == 3)
        {
            Full.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Half.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Low.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            return;
        }
        else if (health == 2)
        {
            Full.transform.localScale = new Vector3(0, 0, 0);
            Half.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Low.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            return;
        }
        else if (health == 1) 
        {
            Full.transform.localScale = new Vector3(0, 0, 0);
            Half.transform.localScale = new Vector3(0, 0, 0);
            Low.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            return;
        }
        return;
    }
}
