using UnityEngine;

public class CounterEnemyQuestStep : QuestStep
{
    [SerializeField] bool pauseinPerfect;
    [SerializeField] bool pauseinSuccess;
    [SerializeField] BattleManager battleManager;
    protected override void DisableStep()
    {
        AIController enemy = battleManager.enemyUnit.GetComponent<AIController>();
        if (enemy != null)
        {
            enemy.IsBeingParried = false;
        }
        QuickTimeEventHandler qte = battleManager.enemyUnit.GetComponent<AIController>().qteHandler;
        if (qte != null)
            qte.OnQTEFinished -= QTEFinished;
    }

    protected override void EnableStep()
    {
        AIController enemy = battleManager.enemyUnit.GetComponent<AIController>();
        if (enemy != null)
        {
            enemy.IsBeingParried = true;
        }
        QuickTimeEventHandler qte = battleManager.enemyUnit.GetComponent<AIController>().qteHandler;
        if (qte != null)
        {
            qte.PauseAtPerfect = pauseinPerfect;
            qte.PauseAtSuccess = pauseinSuccess;
            qte.OnQTEFinished += QTEFinished;
        }
    }

    void QTEFinished(QuickTimeEventResult result)
    {
        bool successCondition = result == QuickTimeEventResult.Success && pauseinSuccess;
        bool perfectCondition = result == QuickTimeEventResult.Perfect && pauseinPerfect;

        if (successCondition || perfectCondition)
        {
            FinishQuestStep();
        }
        else if (result == QuickTimeEventResult.Failed)
        {
            FailedQuestStep();
        }
    }
}