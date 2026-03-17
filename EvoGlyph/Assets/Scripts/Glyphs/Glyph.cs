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
    private SpellController currentController;
    SpellCircle currentSpellCircle;
    public void Activate(Unit user)
    {
        Unit target = user.SelectedTarget;
        if (target == null) return;

        currentController = Instantiate(spellToCast.ControllerPrefab).GetComponent<SpellController>();
        currentController.OnSpellResolved += HandleSpellResolved;
        currentController.Initialize(user, spellToCast);
    }

    private void HandleSpellResolved()
    {
        if (currentSpellCircle != null)
            currentController.OnSpellResolved -= HandleSpellResolved;

        OnGlyphResolved?.Invoke();
    }
}