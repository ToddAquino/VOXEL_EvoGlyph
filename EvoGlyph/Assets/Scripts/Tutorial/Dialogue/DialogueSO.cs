using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "ScriptableObjects/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] dialogueLines;
}


