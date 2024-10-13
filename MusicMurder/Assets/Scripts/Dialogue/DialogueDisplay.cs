using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text lineName, lineText;

    public void Set(string name, string text)
    {
        lineName.text = name;
        lineText.text = text;
    }
}
