using UnityEngine;

public class ApplyDamageEffect : SpellEffect
{
    public int DamageAmount;

    public override void Apply(GameObject target, SpellController controller)
    {
        var damageable = target.GetComponent<IDamageable>();
        float multiplier = 1f;
        ElementType attackingElement = controller.ElementType;
        EnemyUnit targetUnit = target.GetComponent<EnemyUnit>();
        if (targetUnit != null)
        {
            ElementType defendingElement = targetUnit.enemyUnitData.Element;
            float elementalMultiplier = GameManager.Instance.ElementHandler.GetEffectiveness(attackingElement, defendingElement);
            multiplier *= elementalMultiplier;
        }
        multiplier *= controller.GetDamageMultiplier();

        int finalDamage = Mathf.RoundToInt(DamageAmount * multiplier);

        damageable?.TakeDamage(finalDamage);
        EffectSuccessfullyApplied();
    }
}
