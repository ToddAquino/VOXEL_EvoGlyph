using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class EndTurnButton : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent OnEndTurn;
    public void OnPointerClick(PointerEventData eventData)
    {
        OnEndTurn?.Invoke();
    }
}
