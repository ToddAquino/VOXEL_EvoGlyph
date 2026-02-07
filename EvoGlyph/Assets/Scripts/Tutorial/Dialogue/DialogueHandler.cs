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
    private bool isTalking;
    private Coroutine typingCoroutine;
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
        isTalking = true;
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

        isTalking = true;
        string line = dialogueLines.Dequeue();
        currentDialogueLine = line;
        tmp_DialogueBox.text = string.Empty;
        tmp_DialogueBox.maxVisibleCharacters = 0;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(CO_TypeLine(line));
    }

    private void EndDialogue()
    {
        OnDialogueEnd?.Invoke();
    }

    IEnumerator CO_TypeLine(string line)
    {
        tmp_DialogueBox.text = currentDialogueLine;
        foreach (char c in line.ToCharArray())
        {
            tmp_DialogueBox.maxVisibleCharacters++;
            yield return new WaitForSeconds(textSpeed);
        }
        isTalking = false;

        if (autoAdvance)
        {
            if(dialogueLines.Count > 0)
            {
                yield return new WaitForSeconds(autoAdvanceDelay);
                NextLine();
            }
            else
            {
                yield return new WaitForSeconds(autoEndDialogueDelay);
                NextLine();
            }
        }
    }
}
