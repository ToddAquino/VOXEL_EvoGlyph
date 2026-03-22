using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DialogueManager : MonoBehaviour
{
    public event Action<bool> OnDialogueInProgress;
    public static DialogueManager Instance;
    public UnityEvent OnConversationEnd;
    [SerializeField] DialogueHandler dialogueHandler;
    [SerializeField] int currentDialogue;
    [SerializeField] List<DialogueSO> dialogueList = new List<DialogueSO>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        currentDialogue = 0;
    }
    public void ActivateDialogue(List<DialogueSO> dialogue)
    {
        OnDialogueInProgress?.Invoke(true);
        dialogueHandler.gameObject.SetActive(true);
        ClearDialogue();
        currentDialogue = 0;
        AddDialogueToList(dialogue);
        ShowDialogue();
    }
    public void AddDialogueToList(List<DialogueSO> dialogue)
    {
        for (int i = 0; i < dialogue.Count; i++)
        {
            dialogueList.Add(dialogue[i]);
        }
    }

    public void ShowDialogue()
    {
        dialogueHandler.StartDialogue(dialogueList[currentDialogue]);
    }

    public void HideDialogueBox()
    {
        dialogueHandler.gameObject.SetActive(false);
    }

    public void NextDialogue()
    {
        if (currentDialogue < dialogueList.Count - 1)
        {
            currentDialogue++;
            ShowDialogue();
        }
        else
        {
            currentDialogue = 0;
            EndConversation();
        }
    }

    public void ClearDialogue()
    {
        for (int i = dialogueList.Count - 1; i >= 0; i--)
        {
            dialogueList.Remove(dialogueList[i]);
        }

        dialogueList.Clear();
    }

    private void EndConversation()
    {
        //ClearDialogue();
        OnDialogueInProgress?.Invoke(false);
        OnConversationEnd?.Invoke();
    }
}
