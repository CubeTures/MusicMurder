using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] bool loadNextSceneInBuildOrderOnTriggerEnter;

    public void Load(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (loadNextSceneInBuildOrderOnTriggerEnter)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Load(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
