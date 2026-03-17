using UnityEngine;

public class DialogueSignalReciever : MonoBehaviour
{
    public DialogueSystem dialogueSystem;

    public void PlayConversation(string id)
    {
        dialogueSystem.PlayDialogue(id);
    }
}
