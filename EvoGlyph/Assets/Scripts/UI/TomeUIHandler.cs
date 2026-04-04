using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TomeUIHandler : MonoBehaviour
{
    [Header("Tome Piece Quest")]
    [SerializeField] Image TomeIcon;
    [SerializeField] Sprite FireTomeIcon;
    [SerializeField] Sprite LightningTomeIcon;
    [SerializeField] Sprite WaterTomeIcon;

    Sprite tomeIconSprite;
    [SerializeField] Image TomePieceQuestPanel;
    [SerializeField] TextMeshProUGUI TomePieceCountTMP;
    [SerializeField] TextMeshProUGUI TomePieceTMP;

    [Header("Build Tome Quest")]
    [SerializeField] Image BuildTomeQuestPanel;
    [SerializeField] TextMeshProUGUI BuildTomeTMP;
    private void OnEnable()
    {
        if (GameManager.Instance?.PlayerData != null)
        {
            GameManager.Instance.PlayerData.OnTomePieceChanged.AddListener(RefreshTomeUI);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance?.PlayerData != null)
        {
            GameManager.Instance.PlayerData.OnTomePieceChanged.RemoveListener(RefreshTomeUI);
        }
    }

    public void RefreshTomeUI()
    {
        ElementType element = GameManager.Instance.ExplorationData.CurrentAreaType;
        int count = GameManager.Instance.PlayerData.GetTomePieceCount(element);
        TomePieceCountTMP.text = $"{count}/3";

        switch(element)
        {
            case ElementType.Fire:
                TomeIcon.sprite = FireTomeIcon;
                break;
            case ElementType.Lightning:
                TomeIcon.sprite = LightningTomeIcon;
                break;
            case ElementType.Water:
                TomeIcon.sprite = WaterTomeIcon;
                break;
        }
    }
}
