using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NotePickup : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject NotePopUpObj;
   
    [SerializeField] MovingPlayerController playerController;
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        canInteract = true;
    //        player = collision.GetComponent<MovingPlayerController>();
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        canInteract = false;
    //        player = null;
    //    }
    //}

    //private void Update()
    //{
    //    if (canInteract)
    //    {
    //        if (Keyboard.current.eKey.wasPressedThisFrame) //Key "E" is pressed
    //        {
    //            //OpenNotePopUp();
    //        }
    //    }
    //}
    //void OpenNotePopUp()
    //{
        
    //}

    public void Interact(MovingPlayerController player)
    {
        playerController = player;
        if (NotePopUpObj.activeSelf == false)
        {
            if (playerController != null)
            {
                playerController.SetPlayerCanMove(false);
            }
            NotePopUpObj.SetActive(true);
        }
    }

    public void CloseNotePopUp()
    {
        if (playerController != null)
        {
            playerController.SetPlayerCanMove(true);
        }
        NotePopUpObj.SetActive(false);
    }
}

public interface IInteractable
{
    void Interact(MovingPlayerController player);
}