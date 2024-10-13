using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    GameState gameState;
    DialogueScript currentScript;
    [SerializeField] DialogueDisplay display;
    CinemachineVirtualCamera virtualCamera;
    const string virtualCameraTag = "VirtualCamera";

    Transform initialFollow, initialLookAt;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Dialogue Manager Instance not Null");
        }
    }

    private void Start()
    {
        virtualCamera = GameObject.FindGameObjectWithTag(virtualCameraTag).GetComponent<CinemachineVirtualCamera>();
        gameState = GameState.Instance;
    }

    private void Update()
    {
        CheckProgression();
    }

    void CheckProgression()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentScript != null)
        {
            LoadNext();
        }
    }

    void LoadNext()
    {
        DialogueLine line = currentScript.GetNextLine();

        if(line != null)
        {
            SetNext(line);
        }
        else
        {
            CleanUp();
        }
    }

    void SetNext(DialogueLine line)
    {
        if (line.focus != null)
        {
            Transform target = GameObject.FindGameObjectWithTag(line.focus).transform;
            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;
        }

        if (!display.gameObject.activeInHierarchy)
        {
            display.gameObject.SetActive(true);
        }

        display.Set(line.name, line.text);
    }

    void CleanUp()
    {
        display.gameObject.SetActive(false);

        virtualCamera.Follow = initialFollow;
        virtualCamera.LookAt = initialLookAt;

        currentScript = null;

        gameState.SetPaused(false);
    }

    public void SetScript(TextAsset asset)
    {
        currentScript = LoadScript(asset);

        initialFollow = virtualCamera.Follow;
        initialLookAt = virtualCamera.LookAt;
        gameState.SetPaused(true);

        LoadNext();
    }

    public static DialogueScript LoadScript(TextAsset textAsset)
    {
        return JsonUtility.FromJson<DialogueScript>(textAsset.text);
    }
}
