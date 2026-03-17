using System;
using UnityEngine;
using UnityEngine.UI;
public class Glyph : MonoBehaviour
{
    public event Action OnGlyphResolved;
    public bool IsActivated = false;
    public GlyphPattern pattern;
    public Sprite GlyphIcon;
    public SpellData spellToCast;
    SpellCircle currentSpellCircle;
    public void Activate(Unit user)
    {
        Unit target = user.SelectedTarget;
        if (target == null) return;

        currentSpellCircle = spellToCast.BeginCasting(user);
        currentSpellCircle.PerformCast(user);
        currentSpellCircle.OnSpellResolved += HandleSpellResolved;
    }

    private void HandleSpellResolved()
    {
        if (currentSpellCircle != null)
            currentSpellCircle.OnSpellResolved -= HandleSpellResolved;

        OnGlyphResolved?.Invoke();
    }
}