using UnityEngine;

public class PlayerCounterPhaseHandler : MonoBehaviour
{
    [SerializeField] Unit player;
    public QuickTimeEventHandler QTEHandler;
    [SerializeField] float QTEDuration;
    [SerializeField] GameObject QTEvisuals;
    public void StartCounterPhase()
    {
        QTEvisuals.gameObject.SetActive(true);
        QTEHandler.StartQuickTimeEvent(QTEDuration);
    }

    public void OnQTEFinish(QuickTimeEventResult result)
    {
        if (result == QuickTimeEventResult.Success)
        {
            player.HealthComponent.ActivateImmunity();
        }
        QTEvisuals.SetActive(false);
        if (BattleManager.Instance != null)
        {
            BattleController battleController = BattleManager.Instance.Controller;
            BattlePhase phase = BattleManager.Instance.Controller.CurrentPhase;
            if (phase == BattlePhase.PlayerCounter || phase == BattlePhase.PlayerAction)
            {
                player.EndTurn(phase);
            }
        }
    }
}
