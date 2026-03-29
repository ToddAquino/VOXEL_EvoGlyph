using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TomePickup : MonoBehaviour, IInteractable
{
    [SerializeField] string pickupID;
    public static System.Action<string> OnTomePickedUp;
    [SerializeField] List<OnPickupDialogues> onPickupDialogues;
    [SerializeField] Glyph spellToUnlock;
    //bool canInteract = false;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        canInteract = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        canInteract = false;
    //    }
    //}

    //private void Update()
    //{
    //    if (canInteract)
    //    {
    //        if (Keyboard.current.eKey.wasPressedThisFrame) //Key "E" is pressed
    //        {
    //            Interact();
    //        }
    //    }
    //}

    public void Initialize()
    {
        gameObject.SetActive(true);
    }
    public void Interact(MovingPlayerController player)
    {
        ShowOnPickupDialogue();
        PlayerData playerData = GameManager.Instance.PlayerData;
        if (playerData != null)
        {
            playerData.UnlockGlyph(spellToUnlock);
            AudioManager.Instance.PlaySFX("pickUp", 0.5f);
            GameManager.Instance.ExplorationData.RegisterLootedTome(this.GetTomeID());
            OnTomePickedUp?.Invoke(GetTomeID());
            Debug.Log($"Player Found: {spellToUnlock}, {playerData.IsUnlocked(spellToUnlock)}");
            gameObject.SetActive(false);
        }
    }
    public string GetTomeID()
    {
        return pickupID;
    }

    void ShowOnPickupDialogue()
    {
        int index = Random.Range(0,onPickupDialogues.Count);
        DialogueManager.Instance.ActivateDialogue(onPickupDialogues[index].dialogues);
    }
}

[System.Serializable]
public class OnPickupDialogues
{
    public List<DialogueSO> dialogues;
}
