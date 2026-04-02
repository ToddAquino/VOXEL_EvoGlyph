using System.Collections.Generic;
using UnityEngine;

public class TomeTowerTutorialLoader : MonoBehaviour, IInteractable
{
    [SerializeField] string SceneToLoad;

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
    }

    public void Interact(MovingPlayerController player)
    {
        if (!canStartTutorial) return;

        GameSceneManager.Instance.LoadScene(SceneToLoad);
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
