using UnityEngine;

public class PlayerCounterPhaseHandler : MonoBehaviour
{
    [SerializeField] Unit player;
    public QuickTimeEventHandler QTEHandler;
    [SerializeField] float QTEDuration;
    [SerializeField] GameObject QTEVisuals;
    public void StartCounterPhase(float duration)
    {
        QTEDuration = duration;
        QTEVisuals.gameObject.SetActive(true);
        QTEHandler.StartQuickTimeEvent(QTEDuration);
    }

    public void OnQTEFinish(QuickTimeEventResult result)
    {
        if (result == QuickTimeEventResult.Success)
        {
            player.HealthComponent.ActivateImmunity();
        }
        QTEVisuals.SetActive(false);
        //if (BattleManager.Instance != null)
        //{
        //    BattleController battleController = BattleManager.Instance.Controller;
        //    BattlePhase phase = BattleManager.Instance.Controller.CurrentPhase;
        //    if (phase == BattlePhase.PlayerCounter || phase == BattlePhase.PlayerAction)
        //    {
        //        player.EndTurn(phase);
        //    }
        //}
    }
}
