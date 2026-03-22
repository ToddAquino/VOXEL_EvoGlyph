using UnityEngine;

public class DefeatEnemyQuestStep : QuestStep
{
    [SerializeField] BattleManager battleManager;
    protected override void EnableStep()
    {
        EnemyUnit enemy = battleManager.enemyUnit;
        if(enemy != null)  
            enemy.HealthComponent.OnDeath.AddListener(EnemyDefeated);
    }
    protected override void DisableStep()
    {
        EnemyUnit enemy = battleManager.enemyUnit;
        if (enemy != null)
            enemy.HealthComponent.OnDeath.RemoveListener(EnemyDefeated);
    }

    private void EnemyDefeated()
    {
        FinishQuestStep();
    }

}
