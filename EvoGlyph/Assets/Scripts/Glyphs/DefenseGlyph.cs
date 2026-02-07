using UnityEngine;

public class DefenseGlyph : Glyph
{
    [SerializeField] private int shield;
    public override void Activate(Unit user)
    {
        base.Activate(user);
        Debug.Log($"<color=red>Player Casts: Defense Glyph, Shields activated</color>");
        user.HealthComponent.ActivateShield();
    }
}
