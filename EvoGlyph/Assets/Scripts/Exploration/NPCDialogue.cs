using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [Header("Dialogue Sets")]
    [SerializeField] List<NPCDialogueSet> dialogueSets;

    [Header("Settings")]
    [SerializeField] bool randomizeDialogue = true;
    [SerializeField] bool loopDialogues = true;

    [SerializeField] MovingPlayerController playerController;

    int currentIndex = 0;

    public void Interact(MovingPlayerController player)
    {
        playerController = player;

        ShowDialogue();

    }

    void ShowDialogue()
    {
        if (dialogueSets == null || dialogueSets.Count == 0)
        {
            Debug.LogWarning($"No dialogue assigned to {gameObject.name}");
            return;
        }

        int index = GetDialogueIndex();

        DialogueManager.Instance.ActivateDialogue(dialogueSets[index].dialogues);
    }
    //void ShowOnPickupDialogue()
    //{
    //    int index = Random.Range(0, dialogueSets.Count);
    //    DialogueManager.Instance.ActivateDialogue(dialogueSets[index].dialogues);
    //}
    int GetDialogueIndex()
    {
        if (randomizeDialogue)
        {
            return Random.Range(0, dialogueSets.Count);
        }

        int index = currentIndex;

        if (loopDialogues)
        {
            currentIndex = (currentIndex + 1) % dialogueSets.Count;
        }
        else
        {
            currentIndex = Mathf.Min(currentIndex + 1, dialogueSets.Count - 1);
        }

        return index;
    }
}

[System.Serializable]
public class NPCDialogueSet
{
    public List<DialogueSO> dialogues;
}