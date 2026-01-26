using UnityEngine;

public class AttackGlyph : Glyph
{
    [SerializeField] private int damage;

    public override void Activate()
    {
        base.Activate();
        Debug.Log($"Player Casts: Attack Glyph, Deals {damage} Damage");
    }
}
