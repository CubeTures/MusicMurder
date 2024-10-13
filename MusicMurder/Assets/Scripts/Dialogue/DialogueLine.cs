using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class DialogueLine
{
    public string name;
    public string text;

#nullable enable
    [SerializeReference] public string? focus;    
}
