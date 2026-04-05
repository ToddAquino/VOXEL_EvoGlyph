using System;
using System.Collections.Generic;
using UnityEngine;

public class TomeTower : MonoBehaviour,IInteractable
{
    public event Action OnUnlock;
    public event Action OnInteract;
    public Glyph spellToUnlock;
    public ElementType TomeElement;
    public int requiredPieceCount = 3;

    [SerializeField] TomeMinigame minigame;
    //public List<TomePiece> piecesCollected = new List<TomePiece>();
    public bool IsUnlocked;
    public bool canMinigameStart = false;
    public bool IsInTutorial = false;
    MovingPlayerController playerController;
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        //foreach (var piece in tomePieces)
        //{
        //    piece.OnPickup += RegisterPieceCollected;
        //    bool isCollected = GameManager.Instance.ExplorationData.isTomePieceCollected(piece.GetTomePieceID());
        //    GetPieceCollected(piece);
        //    piece.Initialize(!isCollected);
        //}
        canMinigameStart = false;
        CheckTomeProgress();
        //CheckTomePieceCollected();
    }
    //void GetPieceCollected(TomePiece piece)
    //{
    //    if (GameManager.Instance.ExplorationData.isTomePieceCollected(piece.GetTomePieceID()))
    //    {
    //        piecesCollected.Add(piece);
    //    }
      
    //}
    public void Interact(MovingPlayerController player)
    {
        if(IsUnlocked || !canMinigameStart) return;
        UIManager.Instance.ExplorationUIController.DeInitialize();
        minigame.Initialize(this);
        playerController = player;
        if (playerController != null)
        {
            playerController.SetPlayerCanMove(false);
        }
        if (canMinigameStart)
        {
            minigame.BeginMinigame();
            if (IsInTutorial)
            {
                OnInteract?.Invoke();
            }
        }
    }
    
    //public void RegisterPieceCollected(TomePiece piece)
    //{
    //    piece.OnPickup -= RegisterPieceCollected;
    //    GameManager.Instance.ExplorationData.RegisterCollectedTomePiece(piece.GetTomePieceID());
    //    if (!piecesCollected.Contains(piece))
    //    {
    //        piecesCollected.Add(piece);
    //    }

    //    CheckTomePieceCollected();
    //}

    //public void CheckTomePieceCollected()
    //{
    //    foreach (var requiredPiece in tomePieces)
    //    {
    //        if (!piecesCollected.Contains(requiredPiece))
    //        {
    //            Debug.Log($"Piece Missing: {requiredPiece}");
    //            return;
    //        }
    //    }
    //    Debug.Log("All Pieces Collected");
    //    canMinigameStart = true;
    //}

    public void CheckTomeProgress()
    {
        PlayerData playerData = GameManager.Instance.PlayerData;
        int count = 0;
        switch(TomeElement)
        {
            case ElementType.Arcane:
                count = playerData.ArcaneTomePieceCount; break;

            case ElementType.Fire:
                count = playerData.FireTomePieceCount; break;

            case ElementType.Lightning:
                count = playerData.LightningTomePieceCount; break;

            case ElementType.Water:
                count = playerData.WaterTomePieceCount; break;

            default:
                count = 0; break;
        }

        if (count >= requiredPieceCount)
        {
            canMinigameStart = true;
        }
    }
    public void SetPlayerCanMove()
    {
        if (playerController != null)
        {
            playerController.SetPlayerCanMove(true);
        }
    }

    public void UnlockSpell()
    {
        IsUnlocked = true;
        PlayerData playerData = GameManager.Instance.PlayerData;
        if (playerData != null)
        {
            playerData.UnlockGlyph(spellToUnlock);
            AudioManager.Instance.PlaySFX("pickUp", 0.5f);
            //GameManager.Instance.ExplorationData.RegisterLootedTome(this.GetTomeID());
            Debug.Log($"Player Found: {spellToUnlock}, {playerData.IsUnlocked(spellToUnlock)}");
        }
        OnUnlock?.Invoke();
    }
}
