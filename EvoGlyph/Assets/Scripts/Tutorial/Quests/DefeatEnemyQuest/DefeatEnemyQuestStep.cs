using UnityEngine;

public class DefeatEnemyQuestStep : QuestStep
{
    [SerializeField] Unit enemy;
    [SerializeField] GlyphController controller;
    protected override void EnableStep()
    {
        enemy.HealthComponent.OnDeath.AddListener(EnemyDefeated);
        controller.OnTimerRanOut.AddListener(EnemySurvived);
    }
    protected override void DisableStep()
    {
        enemy.HealthComponent.OnDeath.RemoveListener(EnemyDefeated);
        controller.OnTimerRanOut.RemoveListener(EnemySurvived);
    }

    private void EnemyDefeated()
    {
        FinishQuestStep();
    }

    private void EnemySurvived()
    {
        FailedQuestStep();
    }
}
