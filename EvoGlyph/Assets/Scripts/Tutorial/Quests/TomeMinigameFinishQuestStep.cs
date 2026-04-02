using UnityEngine;

public class TomeMinigameFinishQuestStep : QuestStep
{
    [SerializeField] TomeBoard minigame;
    protected override void EnableStep()
    {
        if (minigame != null)
            minigame.OnFinished += MinigameEnded;
    }
    protected override void DisableStep()
    {
        if (minigame != null)
            minigame.OnFinished -= MinigameEnded;
    }
    private void MinigameEnded()
    {
        FinishQuestStep();
    }
}
