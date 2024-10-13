using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : MonoBehaviour
{
    [SerializeField] TextAsset script;
    [SerializeField] bool useTriggerEnter2D;
    [SerializeField] bool retriggerable;
    const string playerTag = "Player";

    public void Trigger()
    {
        DialogueManager.Instance.SetScript(script);

        if(!retriggerable)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (useTriggerEnter2D && collision.gameObject.CompareTag(playerTag))
        {
            Trigger();
        }
    }
}
