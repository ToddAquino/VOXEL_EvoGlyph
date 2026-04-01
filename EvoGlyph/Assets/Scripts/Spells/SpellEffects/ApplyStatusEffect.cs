using UnityEngine;

public class ApplyStatusEffect : SpellEffect
{
    public StatusEffectData StatusEffect;
    public override void Apply(GameObject target, SpellController controller)
    {
        var statusEffectComponent = target.GetComponent<StatusEffectComponent>();
        statusEffectComponent?.ApplyStatusElement(StatusEffect);
        EffectSuccessfullyApplied();
    }
}
