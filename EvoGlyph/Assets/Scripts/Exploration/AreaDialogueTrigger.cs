using System.Collections.Generic;
using UnityEngine;

public class AreaDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Sets")]
    [SerializeField] private List<AreaDialogueSet> dialogueSets;

    [Header("Trigger Settings")]
    [SerializeField] private TriggerMode triggerMode = TriggerMode.OnEnter;
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private bool randomizeDialogue = false;

    [Header("Cooldown Settings")]
    [SerializeField] private bool useCooldown = false;
    [SerializeField] private float cooldownTime = 5f;

    [Header("Audio")]
    [SerializeField] private string triggerSoundKey;
    [SerializeField] private float soundVolume = 0.7f;

    [Header("Visual Feedback")]
    [SerializeField] private bool showGizmo = true;
    [SerializeField] private Color gizmoColor = new Color(0f, 1f, 1f, 0.3f);

    private bool hasTriggered = false;
    private float lastTriggerTime = -999f;
    private int currentDialogueIndex = 0;
    private MovingPlayerController playerInTrigger;

    public enum TriggerMode
    {
        OnEnter,        // Triggers when player enters
        OnExit,         // Triggers when player exits
        OnStay,         // Triggers while player is inside (with cooldown)
        Manual          // Triggered by external script
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInTrigger = other.GetComponent<MovingPlayerController>();

        if (triggerMode == TriggerMode.OnEnter)
        {
            TriggerDialogue();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (triggerMode == TriggerMode.OnStay)
        {
            if (CanTrigger())
            {
                TriggerDialogue();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (triggerMode == TriggerMode.OnExit)
        {
            TriggerDialogue();
        }

        playerInTrigger = null;
    }

    public void TriggerDialogue()
    {
        if (!CanTrigger()) return;

        UIManager.Instance.ExplorationUIController.DeInitialize();
        if (dialogueSets == null || dialogueSets.Count == 0)
        {
            Debug.LogWarning($"No dialogue assigned to area trigger: {gameObject.name}");
            return;
        }

        int index = GetDialogueIndex();
        AreaDialogueSet selectedSet = dialogueSets[index];

        if (selectedSet.dialogues != null && selectedSet.dialogues.Count > 0)
        {
            // Play sound if specified
            if (!string.IsNullOrEmpty(triggerSoundKey))
            {
                AudioManager.Instance?.PlaySFX(triggerSoundKey, soundVolume);
            }

            DialogueManager.Instance?.OnConversationEnd.AddListener(ShowExplorerUI);

            // Show dialogue
            DialogueManager.Instance?.ActivateDialogue(selectedSet.dialogues);

            // Mark as triggered
            hasTriggered = true;
            lastTriggerTime = Time.time;

            Debug.Log($"Area dialogue triggered: {gameObject.name} - Set {index}");
        }
    }

    private void ShowExplorerUI()
    {
        DialogueManager.Instance?.OnConversationEnd.RemoveListener(ShowExplorerUI);
        UIManager.Instance.ShowExplorationUI();
    }

    private bool CanTrigger()
    {
        // Check if already triggered (if trigger once is enabled)
        if (triggerOnce && hasTriggered)
            return false;

        // Check cooldown
        if (useCooldown && Time.time - lastTriggerTime < cooldownTime)
            return false;

        // Check if DialogueManager exists
        if (DialogueManager.Instance == null)
        {
            Debug.LogWarning("DialogueManager.Instance is null!");
            return false;
        }

        return true;
    }

    private int GetDialogueIndex()
    {
        if (randomizeDialogue)
        {
            return Random.Range(0, dialogueSets.Count);
        }

        // Sequential mode
        int index = currentDialogueIndex;
        currentDialogueIndex = (currentDialogueIndex + 1) % dialogueSets.Count;
        return index;
    }

    // Manual trigger from external scripts
    public void ManualTrigger()
    {
        if (triggerMode == TriggerMode.Manual)
        {
            TriggerDialogue();
        }
    }

    // Reset the trigger so it can be used again
    public void ResetTrigger()
    {
        hasTriggered = false;
        currentDialogueIndex = 0;
        lastTriggerTime = -999f;
    }

    // Check if this trigger has been used
    public bool HasBeenTriggered() => hasTriggered;

    private void OnDrawGizmos()
    {
        if (!showGizmo) return;

        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return;

        Gizmos.color = gizmoColor;

        if (col is BoxCollider2D box)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.offset, box.size);

            // Draw wireframe
            Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1f);
            Gizmos.DrawWireCube(box.offset, box.size);
        }
        else if (col is CircleCollider2D circle)
        {
            Gizmos.DrawSphere(transform.position + (Vector3)circle.offset, circle.radius);

            // Draw wireframe
            Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 1f);
            Gizmos.DrawWireSphere(transform.position + (Vector3)circle.offset, circle.radius);
        }

        // Draw label in editor
#if UNITY_EDITOR
        UnityEditor.Handles.Label(
            transform.position + Vector3.up * 0.5f,
            $"[{triggerMode}] {gameObject.name}",
            new GUIStyle()
            {
                normal = new GUIStyleState() { textColor = Color.cyan },
                alignment = TextAnchor.MiddleCenter,
                fontSize = 10
            }
        );
#endif
    }
}

[System.Serializable]
public class AreaDialogueSet
{
    [TextArea(2, 3)]
    public string editorNote; // For organization in Inspector
    public List<DialogueSO> dialogues;
}