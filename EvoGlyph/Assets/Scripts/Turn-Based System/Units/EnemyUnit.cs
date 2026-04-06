using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : Unit
{
    public EnemyUnitData enemyUnitData;
    public int level;
    [SerializeField] Image elementIconRenderer;

    //public List<SpellData> spellOptions = new List<SpellData>();
    public SpellData spellToCast;
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
        elementIconRenderer.sprite = enemyData.Element.Icon;
        spellToCast = enemyData.spellToCast;
        base.Initialize();
        //spellOptions = new List<SpellDefinition>(enemyUnitData.spells);

    }
    public override void CheckConditions()
    {
        StatusEffectComponent statusComp = GetComponent<StatusEffectComponent>();
        if (statusComp == null) return;

        bool hasArcane = HasStatus(statusComp, StatusEffect.Arcane);

        bool isGhost = enemyUnitData != null &&
                       enemyUnitData.Element != null &&
                       enemyUnitData.Element.Type == ElementType.Ghost;
        bool isFire = enemyUnitData != null &&
               enemyUnitData.Element != null &&
               enemyUnitData.Element.Type == ElementType.Fire;

        if (!isGhost && !hasArcane)
        {
            hasArcane = false;
            RemoveStatus(statusComp, StatusEffect.Arcane);
        }


        if (hasArcane && isGhost)
        {
            if (UnityEngine.Random.value <= 0.5f)
            {
                int bonusDamage = 10;
                HealthComponent.TakeDamage(bonusDamage);

                //Debug.Log("Arcane + Ghost triggered!");
                RemoveStatus(statusComp, StatusEffect.Arcane);
            }
        }
        if (isFire && HasStatus(statusComp, StatusEffect.Burning))
        {
            RemoveStatus(statusComp, StatusEffect.Burning);
        }
        base.CheckConditions(); 


    }
    //public SpellData GetRandomSpellToCast()
    //{
    //    if (spellOptions.Count == 0) return null;

    //    int index = Random.Range(0, spellOptions.Count);

    //    return spellOptions[index];
    //}

    public override void OnDeath()
    {
        base.OnDeath();
        Deinitialize();
    }
}
