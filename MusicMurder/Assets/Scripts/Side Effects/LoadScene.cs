using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void Load(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
