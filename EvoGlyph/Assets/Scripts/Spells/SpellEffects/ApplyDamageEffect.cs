using UnityEngine;

public class ApplyDamageEffect : SpellEffect
{
    public int DamageAmount;

    public override void Apply(GameObject target)
    {
        var damageable = target.GetComponent<IDamageable>();
        Spell parentSpell = GetComponentInParent<Spell>();
        float multiplier = 1f;

        if (parentSpell != null)
            multiplier = parentSpell.damageMultiplier;

        int finalDamage = Mathf.RoundToInt(DamageAmount * multiplier);

        damageable?.TakeDamage(finalDamage);
        EffectSuccessfullyApplied();
    }
}
