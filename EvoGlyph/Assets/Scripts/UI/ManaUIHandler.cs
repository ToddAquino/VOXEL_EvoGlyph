using UnityEngine;
using UnityEngine.UI;
public class ManaUIHandler : MonoBehaviour
{
    [SerializeField] Image[] manaPoints;
    [SerializeField] Sprite litSprite;
    [SerializeField] Sprite unlitSprite;
    private void OnEnable()
    {
        if (GameManager.Instance?.PlayerData != null)
        {
            GameManager.Instance.PlayerData.OnManaChanged.AddListener(RefreshManaUI);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance?.PlayerData != null)
        {
            GameManager.Instance.PlayerData.OnManaChanged.RemoveListener(RefreshManaUI);
        }
    }
    private void Start()
    {
        RefreshManaUI();
    }

    public void RefreshManaUI()
    {
        int manaCount = GameManager.Instance.PlayerData.CurrentMana;
        for (int i = 0; i < manaPoints.Length; i++)
        {
            if(manaPoints[i] != null && manaPoints[i].gameObject.activeSelf)
            {
                if (manaCount > i)
                {
                    manaPoints[i].sprite = litSprite;
                }
                else
                {
                    manaPoints[i].sprite = unlitSprite;
                }
            }
        }
    }
}
