using UnityEngine;

public class ApplyDamageEffect : SpellEffect
{
    public int DamageAmount;

    public override void Apply(GameObject target)
    {
        var damageable = target.GetComponent<IDamageable>();
        damageable?.TakeDamage(DamageAmount);
    }
}
