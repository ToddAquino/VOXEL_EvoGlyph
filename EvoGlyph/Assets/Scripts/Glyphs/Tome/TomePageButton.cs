using UnityEngine;
using UnityEngine.EventSystems;

public class TomePageButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] bool isRightPage;
    [SerializeField] TomeController tomeController;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (tomeController == null) return;

        if (isRightPage)
            tomeController.NextPages();
        else
            tomeController.PreviousPages();
    }
}
