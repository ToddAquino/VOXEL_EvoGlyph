using UnityEngine;
using UnityEngine.InputSystem;

public class ManaTower : MonoBehaviour
{
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

    void Interact()
    {
        Debug.Log("Refill all Player mana");
        PlayerData playerData = GameManager.Instance.PlayerData;
        if (playerData != null)
        {
            playerData.RefillMana(playerData.MaxMana);
        }
    }
}
