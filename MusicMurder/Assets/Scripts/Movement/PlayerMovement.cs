using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : Living
{
    public static PlayerMovement Instance { get; private set; }

    public delegate void PlayerAction(PlayerActionType actionType, float timestamp);
    PlayerAction onPlayerAction;
    PlayerTempo tempo;
    public Accuracy acc { get; private set; }

    [SerializeField] GameObject deathAnimation;
    [SerializeField] GameObject curtain;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Player Instance not Null");
        }
    }

    private new void Start()
    {
        base.Start();
        Health = 3;
        tempo = PlayerTempo.Instance;
        tempo.ListenOnPlayerAccuracy(GetAccuracy);
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (!canAct) return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            direction.y = 1;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction.y = -1;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction.x = -1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction.x = 1;
        }
    }

    override protected void OnMove()
    {
        NotifyOnPlayerAction(PlayerActionType.MOVE);
    }

    public void ListenOnPlayerAction(PlayerAction p)
    {
        onPlayerAction += p;
    }

    public void UnlistenOnPlayerAction(PlayerAction p)
    {
        onPlayerAction -= p;
    }

    void NotifyOnPlayerAction(PlayerActionType actionType)
    {
        float timestamp = Time.time;
        foreach (PlayerAction p in onPlayerAction.GetInvocationList())
        {
            p.Invoke(actionType, timestamp);
        }
    }

    void GetAccuracy(Accuracy accuracy)
    {
        acc = accuracy;
    }

    public void Death()
    {
        StartCoroutine(LoadDeathScreen());
    }

    public IEnumerator LoadDeathScreen()
    {
        gameState.SetPaused(true);
        gameState.SetFreeze(true);

        GameObject temp = Instantiate(curtain, new Vector2(currentTile.x, currentTile.y), Quaternion.identity) as GameObject;
        spriteRenderer.sortingOrder = 103;

        GameObject.Find("Music").GetComponent<AudioSource>().Pause();

        yield return new WaitForSeconds(1.5f);

        spriteRenderer.sortingOrder = 10;
        spriteRenderer.sortingOrder = 10;

        GameObject death = Instantiate(deathAnimation, new Vector2(currentTile.x, currentTile.y), Quaternion.identity) as GameObject;
        death.GetComponent<SpriteRenderer>().sortingOrder = 102;

        yield return new WaitForSeconds(1f);

        DeathScreen.OriginScene = SceneManager.GetActiveScene().name;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Death");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        print("death done");
    }
}

public enum PlayerActionType
{
    MOVE,
    ATTACK,
}
