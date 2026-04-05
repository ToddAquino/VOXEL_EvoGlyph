using System;
using UnityEngine;

public class ManaTower : MonoBehaviour, IInteractable
{
    public event Action OnInteract;
    [SerializeField] ManaTowerMinigame minigame;
    public Glyph RequiredGlyph;
    MovingPlayerController playerController;
    public bool IsInTutorial = false;
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

    public void Interact(MovingPlayerController player)
    {
        UIManager.Instance.ExplorationUIController.DeInitialize();
        minigame.Initialize(this);
        playerController = player;
        if (playerController != null)
        {
            playerController.SetPlayerCanMove(false);
        }
        if (IsInTutorial)
        {
            OnInteract?.Invoke();
        }
    }

    public void RefillMana()
    {
        Debug.Log("Refill all Player mana");
        PlayerData playerData = GameManager.Instance.PlayerData;
        if (playerData != null)
        {
            playerData.RefillMana(playerData.MaxMana);
        }
    }

    public void SetPlayerCanMove()
    {
        if (playerController != null)
        {
            playerController.SetPlayerCanMove(true);
        }
    }
}
