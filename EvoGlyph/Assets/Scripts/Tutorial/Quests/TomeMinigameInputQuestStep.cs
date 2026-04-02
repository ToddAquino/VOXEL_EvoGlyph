using UnityEngine;

public class TomeMinigameInputQuestStep : QuestStep
{
    [SerializeField] TomeBoard minigame;
    protected override void EnableStep()
    {
        if (minigame != null)
            minigame.OnNodeInput += InputDone;
    }
    protected override void DisableStep()
    {
        if (minigame != null)
            minigame.OnNodeInput -= InputDone;
    }
    private void InputDone()
    {
        FinishQuestStep();
    }
}
