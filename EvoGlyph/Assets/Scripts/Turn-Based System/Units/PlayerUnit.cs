using UnityEngine;

public class PlayerUnit : Unit
{
    public PlayerData playerData;

    private void Start()
    {
        playerData = GameManager.Instance.PlayerData;
    }
    public override void Initialize()
    {
        //SetMaxHealth
        HealthComponent.SetMaxHealth(playerData.maxHP); 
        base.Initialize();
        Team = Team.Player;

    }

    public void PerformBasicAttack()
    {
        Unit target = GetTarget();
        if (target == null) return;

        int damage = playerData.baseAttack;

        var damageable = target.GetComponent<IDamageable>();
        float multiplier = 1f;
        ElementType attackingElement = ElementType.None;
        EnemyUnit targetUnit = target.GetComponent<EnemyUnit>();
        if (targetUnit != null)
        {
            ElementType defendingElement = targetUnit.enemyUnitData.Element.Type;
            float elementalMultiplier = GameManager.Instance.ElementHandler.GetEffectiveness(attackingElement, defendingElement);
            multiplier *= elementalMultiplier;
        }
        int finalDamage = Mathf.RoundToInt(damage * multiplier);

        damageable?.TakeDamage(finalDamage);
    }

    public void GainMana(int amount)
    {
        GameManager.Instance.PlayerData.RefillMana(amount);
    }
}
