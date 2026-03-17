using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public DialogueDatabase database;
    Coroutine currentConversation;
    public void PlayDialogue(string id)
    {
        if (currentConversation != null)
            StopCoroutine(currentConversation);

        currentConversation = StartCoroutine(RunConversation(id));
    }

    IEnumerator RunConversation(string id)
    {
        Conversation convo = database.Get(id);

        if (convo == null)
        {
            Debug.LogWarning("Conversation not found: " + id);
            yield break;
        }

        foreach (Dialogueline line in convo.lines)
        {
            ShowLine(line);

            float duration = GetLineDuration(line);

            yield return new WaitForSeconds(duration);
        }

        dialogueText.text = "";
    }

    void ShowLine(Dialogueline line)
    {
        dialogueText.text = line.text;
    }

    float GetLineDuration(Dialogueline line)
    {
        return Mathf.Max(2f, line.text.Length * 0.05f);
    }
}
