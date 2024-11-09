using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    Transform canvas;
    GameObject pausePrefab;
    GameObject liveScreen;
    GameState gameState;

    void Start()
    {
        gameState = GameState.Instance;
        canvas = transform;
        pausePrefab = Resources.Load<GameObject>("PauseScreen");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (liveScreen != null && liveScreen.activeInHierarchy)
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
        if (liveScreen == null)
        {
            liveScreen = Instantiate(pausePrefab, canvas);
            Button button = liveScreen.GetComponentInChildren<Button>(true);
            button.onClick.AddListener(Unpause);
        }
        else
        {
            liveScreen.SetActive(true);
        }

        gameState.SetPaused(true);
    }

    void Unpause()
    {
        liveScreen.SetActive(false);
        gameState.SetPaused(false);
    }
}
