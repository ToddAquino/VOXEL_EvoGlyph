using TMPro;
using UnityEngine;

public class TomeUIHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fireTomePieceCountTMP;
    [SerializeField] TextMeshProUGUI LightningTomePieceCountTMP;
    [SerializeField] TextMeshProUGUI WaterTomePieceCountTMP;
    [SerializeField] int maxPieceCount = 3;
    private void OnEnable()
    {
        //if (GameManager.Instance?.PlayerData != null)
        //{
        //    GameManager.Instance.PlayerData.OnManaChanged.AddListener(RefreshManaUI);
        //}
    }

    private void OnDisable()
    {
        //if (GameManager.Instance?.PlayerData != null)
        //{
        //    GameManager.Instance.PlayerData.OnManaChanged.RemoveListener(RefreshManaUI);
        //}
    }
}
