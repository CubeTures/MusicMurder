using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueScript
{
    public DialogueLine[] lines;

    int counter = 0;

    public DialogueLine GetNextLine()
    {
        return counter < lines.Length ? lines[counter++] : null;
    }
}
