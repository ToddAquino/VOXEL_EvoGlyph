using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class DialogueHandler : MonoBehaviour
{
    public UnityEvent OnDialogueEnd;
    [SerializeField] TextMeshProUGUI tmp_DialogueBox;

    [Header("Dialogue Config")]
    [SerializeField] bool autoAdvance;
    [SerializeField] float autoAdvanceDelay;
    [SerializeField] float autoEndDialogueDelay;
    [SerializeField] float textSpeed;
    [SerializeField] string currentDialogueLine;

    private Queue<string> dialogueLines;

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    //throw new System.NotImplementedException();
    //    if (eventData.button == PointerEventData.InputButton.Left)
    //    {
    //        if (!isTalking)
    //        {
    //            NextLine();
    //        }
    //        else
    //        {
    //            StopCoroutine(typingCoroutine);
    //            tmp_DialogueBox.maxVisibleCharacters = currentDialogueLine.Length;
    //            isTalking = false;
    //        }
    //    }
    //}
    void Awake()
    {
        dialogueLines = new Queue<string>();
        tmp_DialogueBox.text = string.Empty;
    }

   public void StartDialogue(DialogueSO dialogueSO)
    {
        dialogueLines.Clear();

        foreach (string line in dialogueSO.dialogueLines)
        {
            dialogueLines.Enqueue(line);
        }
        NextLine();
    }

    public void NextLine()
    {
        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        tmp_DialogueBox.text = string.Empty;

        string line = dialogueLines.Dequeue();
        currentDialogueLine = line;
        tmp_DialogueBox.text = currentDialogueLine;
    }

    private void EndDialogue()
    {
        OnDialogueEnd?.Invoke();
    }

}
