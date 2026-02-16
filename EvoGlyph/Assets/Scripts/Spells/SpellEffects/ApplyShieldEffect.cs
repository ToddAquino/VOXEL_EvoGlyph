using UnityEngine;

public class ApplyShieldEffect : SpellEffect
{
    [SerializeField] float damageReductionRate;
    public override void Apply(GameObject target)
    {
        var health = target.GetComponent<HealthComponent>();
        health?.ActivateBarrierAbility(damageReductionRate);
    }
}
