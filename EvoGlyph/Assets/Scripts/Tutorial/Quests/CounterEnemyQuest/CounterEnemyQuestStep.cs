using UnityEngine;

public class CounterEnemyQuestStep : QuestStep
{
    [SerializeField] QuickTimeEventHandler QTEHandler;
    protected override void DisableStep()
    {
        QTEHandler.OnQTEFinished -= QTEFinished;
    }

    protected override void EnableStep()
    {
        QTEHandler.OnQTEFinished += QTEFinished;
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
