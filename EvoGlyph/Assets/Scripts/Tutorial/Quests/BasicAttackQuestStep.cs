using UnityEngine;

public class BasicAttackQuestStep : QuestStep
{
    [SerializeField] BattleManager battleManager;

    protected override void DisableStep()
    {
        BasicAttackQTE qte = battleManager.playerUnit.GetComponent<PlayerController>().qteHandler;
        if (qte != null)
            qte.OnQTEFinished -= QTEFinished;
    }
    protected override void EnableStep()
    {
        BasicAttackQTE qte = battleManager.playerUnit.GetComponent<PlayerController>().qteHandler;
        if (qte != null)
        {
            qte.OnQTEFinished += QTEFinished;
        }
    }
    void QTEFinished(QuickTimeEventResult result)
    {
        if (result == QuickTimeEventResult.Success || result == QuickTimeEventResult.Perfect)
        {
            FinishQuestStep();
        }
        else if (result == QuickTimeEventResult.Failed)
        {
            FailedQuestStep();
        }
    }
}
