using System;
using UnityEngine;

public class TomePiece : MonoBehaviour,IInteractable
{
    [SerializeField] string pieceID;
    public event Action<TomePiece> OnPickup;
    public bool canPickup = false;
    [SerializeField] SpriteRenderer pickupSprite;

    public string GetTomePieceID()
    {
        return pieceID;
    }

    public void Initialize(bool isActive)
    {
        canPickup = isActive;
        pickupSprite.enabled = isActive;
        this.GetComponent<BoxCollider2D>().enabled = isActive;
    }

    public void Interact(MovingPlayerController player)
    {
        if(!canPickup) return;
        PickupTome();
    }

    void PickupTome()
    {
        canPickup = false;
        pickupSprite.enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        OnPickup?.Invoke(this);
    }
}
