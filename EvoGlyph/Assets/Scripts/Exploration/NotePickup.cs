using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NotePickup : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject NotePopUpObj;
    [SerializeField] TomePiece[] tomePieces;

    [SerializeField] MovingPlayerController playerController;

    //area0
    [SerializeField] AreaDialogueTrigger kap3TriggerDialogue;
    [SerializeField] GameObject kap4TriggerDialogue;

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
    private void Start()
    {
        foreach (var piece in tomePieces)
        {
            piece.gameObject.SetActive(false);
        }
    }

    public void Interact(MovingPlayerController player)
    {
        UIManager.Instance.ExplorationUIController.DeInitialize();
        playerController = player;
        AudioManager.Instance.PlaySFX("pageTurn", 0.5f); //assuming note is made of paper visually of course
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
        UIManager.Instance.ExplorationUIController.Initialize();
        if (playerController != null)
        {
            playerController.SetPlayerCanMove(true);
        }
        AudioManager.Instance.PlaySFX("click", 0.5f);
        NotePopUpObj.SetActive(false);
        ShowTomePieces();
        //dialogue
        kap3TriggerDialogue.ManualTrigger();
        kap4TriggerDialogue.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ShowTomePieces()
    {
        foreach(var piece in tomePieces)
        {
            piece.gameObject.SetActive(true);
        }
    }
}

public interface IInteractable
{
    void Interact(MovingPlayerController player);
}