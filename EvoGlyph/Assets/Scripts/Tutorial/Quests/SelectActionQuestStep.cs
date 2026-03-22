using UnityEngine;

public class SelectActionQuestStep : QuestStep
{
    [SerializeField] BattleManager battleManager;
    [SerializeField] ActionPicked requiredAction;
    protected override void DisableStep()
    {
        PlayerUnit player = battleManager.playerUnit;
        player.GetComponent<PlayerController>().OnActionPicked -= ActionPerformed;
    }

    protected override void EnableStep()
    {
        PlayerUnit player = battleManager.playerUnit;
        player.GetComponent<PlayerController>().OnActionPicked += ActionPerformed;
    }

    void ActionPerformed(ActionPicked action)
    {
        if (action == requiredAction)
        {
            FinishQuestStep();
        }
    }
}
