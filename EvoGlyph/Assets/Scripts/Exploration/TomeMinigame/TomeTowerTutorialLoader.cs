using System.Collections.Generic;
using UnityEngine;

public class TomeTowerTutorialLoader : MonoBehaviour, IInteractable
{
    [SerializeField] Tutorial tutorial;
    [SerializeField] TomeTower tower;
    //public ElementType TomeElement;
    //public int requiredPieceCount = 3;
    [SerializeField] TomePiece[] tomePieces;
    public List<TomePiece> piecesCollected = new List<TomePiece>();
    public bool canStartTutorial = false;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        foreach (var piece in tomePieces)
        {
            piece.OnPickup += RegisterPieceCollected;
            bool isCollected = GameManager.Instance.ExplorationData.isTomePieceCollected(piece.GetTomePieceID());
            GetPieceCollected(piece);
            piece.Initialize(!isCollected);
        }
        canStartTutorial = false;
        //CheckTomeProgress();
    }

    public void Interact(MovingPlayerController player)
    {
        if (!canStartTutorial) return;
        tutorial.gameObject.SetActive(true);
        tower.CheckTomeProgress();
        tower.Interact(player);
    }
    void GetPieceCollected(TomePiece piece)
    {
        if (GameManager.Instance.ExplorationData.isTomePieceCollected(piece.GetTomePieceID()))
        {
            piecesCollected.Add(piece);
        }

    }
    public void RegisterPieceCollected(TomePiece piece)
    {
        piece.OnPickup -= RegisterPieceCollected;
        GameManager.Instance.ExplorationData.RegisterCollectedTomePiece(piece.GetTomePieceID());
        if (!piecesCollected.Contains(piece))
        {
            piecesCollected.Add(piece);
        }

        CheckTomePieceCollected();
    }
    //public void CheckTomeProgress()
    //{
    //    PlayerData playerData = GameManager.Instance.PlayerData;
    //    int count = 0;
    //    switch (TomeElement)
    //    {
    //        case ElementType.Arcane:
    //            count = playerData.ArcaneTomePieceCount; break;

    //        case ElementType.Fire:
    //            count = playerData.FireTomePieceCount; break;

    //        case ElementType.Lightning:
    //            count = playerData.LightningTomePieceCount; break;

    //        case ElementType.Water:
    //            count = playerData.WaterTomePieceCount; break;

    //        default:
    //            count = 0; break;
    //    }

    //    if (count >= requiredPieceCount)
    //    {
    //        canStartTutorial = true;
    //    }
    //}
    public void CheckTomePieceCollected()
    {
        foreach (var requiredPiece in tomePieces)
        {
            if (!piecesCollected.Contains(requiredPiece))
            {
                Debug.Log($"Piece Missing: {requiredPiece}");
                return;
            }
        }
        Debug.Log("All Pieces Collected");
        canStartTutorial = true;
    }
}
