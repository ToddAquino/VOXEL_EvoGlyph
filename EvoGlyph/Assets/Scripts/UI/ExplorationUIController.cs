using UnityEngine;

public class ExplorationUIController : MonoBehaviour
{
    [SerializeField] ManaUIHandler manaUIHandler;
    [SerializeField] TomeUIHandler tomeUIHandler;
    public void Initialize()
    {
        ShowManaUI();
        manaUIHandler.RefreshManaUI();
        ShowTomeTracker();
        tomeUIHandler.RefreshTomeUI();
    }

    public void DeInitialize()
    {
        HideManaUI();
        HideTomeTracker();
    }

    public void ShowManaUI()
    {
        manaUIHandler.gameObject.SetActive(true);
    }

    public void HideManaUI()
    {
        manaUIHandler.gameObject.SetActive(false);
    }

    public void ShowTomeTracker()
    {
        tomeUIHandler.gameObject.SetActive(true);
    }

    public void HideTomeTracker()
    {
        tomeUIHandler.gameObject.SetActive(false);
    }
}
