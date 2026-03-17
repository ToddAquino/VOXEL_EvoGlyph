using UnityEngine;

public class ApplyStatusEffect : SpellEffect
{
    public ElementType Element;
    public override void Apply(GameObject target, SpellController controller)
    {
        var statusEffectComponent = target.GetComponent<StatusEffectComponent>();
        statusEffectComponent?.ApplyStatusElement(Element);
        EffectSuccessfullyApplied();
    }
}
