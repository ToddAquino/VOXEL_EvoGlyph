using UnityEngine;

public class ApplyStatusEffect : SpellEffect
{
    public ElementType Element;
    public override void Apply(GameObject target)
    {
        var statusEffectComponent = target.GetComponent<StatusEffectComponent>();
        statusEffectComponent?.ApplyStatusElement(Element);
        EffectSuccessfullyApplied();
    }
}
