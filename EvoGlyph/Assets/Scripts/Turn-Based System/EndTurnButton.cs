using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class EndTurnButton : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent<BattlePhase> OnEndTurn;
    public void OnPointerClick(PointerEventData eventData)
    {
        BattlePhase phase = BattleManager.Instance.Controller.CurrentPhase;
        if (phase == BattlePhase.PlayerCounter || phase == BattlePhase.PlayerAction)
            OnEndTurn?.Invoke(phase);
    }
}
