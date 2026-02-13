using UnityEngine;

public class ApplyShieldEffect : SpellEffect
{
    public override void Apply(GameObject target)
    {
        var shieldable = target.GetComponent<IShieldable>();
        shieldable?.ActivateShield();
    }
}
