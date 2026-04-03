using UnityEngine;

public class ManaMinigameFinishQuestStep : QuestStep
{
    [SerializeField] ManaTowerMinigame minigame;
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
