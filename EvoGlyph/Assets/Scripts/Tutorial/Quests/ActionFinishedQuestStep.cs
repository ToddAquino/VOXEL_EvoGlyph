using System.Collections.Generic;
using UnityEngine;

public class ActionFinishedQuestStep : QuestStep
{
    [SerializeField] BattleManager battleManager;
    protected override void EnableStep()
    {
        PlayerUnit player = battleManager.playerUnit;
        if (player != null)
            player.GetComponent<PlayerController>().OnActionEnded += ActionEnded;
    }
    protected override void DisableStep()
    {
        PlayerUnit player = battleManager.playerUnit;
        if (player != null)
            player.GetComponent<PlayerController>().OnActionEnded -= ActionEnded;
    }
    private void ActionEnded()
    {
        FinishQuestStep();
    }
}
