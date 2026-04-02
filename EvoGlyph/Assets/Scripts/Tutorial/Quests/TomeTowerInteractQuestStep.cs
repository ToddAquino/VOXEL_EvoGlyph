using UnityEngine;

public class TomeTowerInteractQuestStep : QuestStep
{
    [SerializeField] TomeTower tower;
    protected override void EnableStep()
    {
        if (tower != null)
           tower.OnInteract += ActionEnded;
    }
    protected override void DisableStep()
    {
        if (tower != null)
            tower.OnInteract -= ActionEnded;
    }
    private void ActionEnded()
    {
        FinishQuestStep();
    }
}
