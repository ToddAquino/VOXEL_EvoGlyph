using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    public EnemyUnitData enemyUnitData;

    public int level;

    public List<SpellData> spellOptions = new List<SpellData>();

    public override void Initialize()
    {
        Team = Team.Enemy;
        if (enemyUnitData == null)
        {
            Debug.LogError("EnemyUnitData is NULL");
            return;
        }
        EnemyUnitData enemyData = GameManager.Instance.ExplorationData.CurrentEncounteredEnemy.GetEnemyData();

        HealthComponent.SetMaxHealth(enemyData.MaxHP);
        
        base.Initialize();
        //spellOptions = new List<SpellDefinition>(enemyUnitData.spells);

    }

    public SpellData GetRandomSpellToCast()
    {
        if (spellOptions.Count == 0) return null;

        int index = Random.Range(0, spellOptions.Count);

        return spellOptions[index];
    }
}
