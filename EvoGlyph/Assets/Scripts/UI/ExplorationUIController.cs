using UnityEngine;

public class ExplorationUIController : MonoBehaviour
{
    [SerializeField] ManaUIHandler manaUIHandler;
    [SerializeField] TomeUIHandler tomeUIHandler;
    [SerializeField] ExplorerUIHandler explorerUIHandler;
    public void Initialize()
    {
        ShowManaUI();
        manaUIHandler.RefreshManaUI();
        ShowTomeTracker();
        tomeUIHandler.RefreshTomeUI();
        ShowExplorerUI();
    }

    public void DeInitialize()
    {
        HideManaUI();
        HideTomeTracker();
        HideExplorerUI();
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

    public void ShowExplorerUI()
    {
        explorerUIHandler.gameObject.SetActive(true) ;
    }

    public void HideExplorerUI()
    {
        explorerUIHandler.gameObject.SetActive(false);
    }
}
