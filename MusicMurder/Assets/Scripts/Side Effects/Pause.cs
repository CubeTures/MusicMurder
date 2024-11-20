using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;
    GameState gameState;

    void Start()
    {
        gameState = GameState.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreen.activeInHierarchy)
            {
                Unpause();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        pauseScreen.SetActive(true);
        gameState.SetPaused(true);
    }

    public void Unpause()
    {
        pauseScreen.SetActive(false);
        gameState.SetPaused(false);
    }

    public void LoadRoom(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
