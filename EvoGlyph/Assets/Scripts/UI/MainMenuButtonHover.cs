using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI highlightText;
    [SerializeField] Color highlightColor = Color.aquamarine;
    [SerializeField] Color normalColor = Color.white;
    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightText.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightText.color = normalColor;
    }

}
