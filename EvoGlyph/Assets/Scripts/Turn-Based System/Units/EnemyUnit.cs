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

    public void CheckConditions()
    {
        StatusEffectComponent statusComp = GetComponent<StatusEffectComponent>();

        if (statusComp == null || statusComp.ActiveStatuses.Count == 0)
        {
            Debug.Log("No status on enemy");
            return; 
        }
        List<StatusEffectData> statuses = statusComp.ActiveStatuses;

        bool hasWet = HasStatus(statusComp, StatusEffect.Wet);
        bool hasBurning = HasStatus(statusComp, StatusEffect.Burning);
        bool hasShocked = HasStatus(statusComp, StatusEffect.Shocked);
        bool hasElectrocute = HasStatus(statusComp, StatusEffect.Electrocute);
        if (hasBurning)
        {
            int burnDamage = Mathf.RoundToInt(6); //HARDCODED DMG, CHANGE IF NEEDED
            HealthComponent.TakeDamage(burnDamage);

            // Burning is extinguished by Wet
            if (hasWet)
            {
                RemoveStatus(statusComp, StatusEffect.Burning);
            }
            Debug.Log("FIRE");
        }
        if (hasElectrocute)
        {
            // Consume Wet for bonus damage
            int shockBonus = Mathf.RoundToInt(20);
            HealthComponent.TakeDamage(shockBonus);
            Debug.Log("SHOCKED");
            RemoveStatus(statusComp, StatusEffect.Electrocute);
        }
        if (hasWet)
        {
            int waterDamage = Mathf.RoundToInt(3);
            HealthComponent.TakeDamage(waterDamage);
            Debug.Log("WET");
            // Wet is removed by Burning
            if (hasBurning)
            {
                Debug.Log("Wet removed!");
                RemoveStatus(statusComp, StatusEffect.Wet);
            }
        }

    }
    private bool HasStatus(StatusEffectComponent comp, StatusEffect effect)
    {
        foreach (var status in comp.ActiveStatuses)
        {
            if (status.Effect == effect)
                return true;
        }
        return false;
    }

    private void RemoveStatus(StatusEffectComponent comp, StatusEffect effect)
    {
        for (int i = comp.ActiveStatuses.Count - 1; i >= 0; i--)
        {
            if (comp.ActiveStatuses[i].Effect == effect)
            {
                comp.RemoveStatus(comp.ActiveStatuses[i]);
            }
        }
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
