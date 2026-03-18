using UnityEngine;
using UnityEngine.InputSystem;

public class TomePickup : MonoBehaviour
{
    [SerializeField] Glyph spellToUnlock;
    bool canInteract = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    private void Update()
    {
        if (canInteract)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame) //Key "E" is pressed
            {
                Interact();
            }
        }
    }
    public string GetTomeID()
    {
        return this.gameObject.name;
    }
    void Interact()
    {        
        PlayerData playerData = GameManager.Instance.PlayerData;
        if (playerData != null)
        {
            playerData.UnlockGlyph(spellToUnlock);
            AudioManager.Instance.PlaySFX("pickUp", 0.5f);
            GameManager.Instance.ExplorationData.RegisterLootedTome(this.GetTomeID());
            Debug.Log($"Player Found: {spellToUnlock}, {playerData.IsUnlocked(spellToUnlock)}");
            gameObject.SetActive(false);
        }
    }
}
