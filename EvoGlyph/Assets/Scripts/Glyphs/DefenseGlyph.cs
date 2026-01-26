using UnityEngine;

public class DefenseGlyph : Glyph
{
    [SerializeField] private int shield;
    public override void Activate()
    {
        base.Activate();
        Debug.Log($"Player Casts: Defense Glyph, gains {shield} Shields");
    }
}
