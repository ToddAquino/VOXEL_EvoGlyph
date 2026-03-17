using System;
using System.Collections;
using UnityEngine;

public class CastSpellAction : MonoBehaviour
{
    public event Action OnActionResolved; 
    public SpellData spellToCast;
    QuickTimeEventResult qteResult;
    SpellCircle currentSpellCircle;
    public void SetQTEResult(QuickTimeEventResult result)
    {
        qteResult = result;
    }
    public void ReleaseSpell(Unit user)
    {
        currentSpellCircle = spellToCast.BeginCasting(user);
        currentSpellCircle.PerformCast(user);

        switch (qteResult)
        {
            case QuickTimeEventResult.Success:
                currentSpellCircle.IsInterrupted = true;
                break;

            case QuickTimeEventResult.Perfect:
                currentSpellCircle.SetDamageMultiplier(0.25f);
                currentSpellCircle.SpellDeflected();
                break;

            default:
                currentSpellCircle.SetDamageMultiplier(1f);
                break;
        }
        currentSpellCircle.OnSpellResolved += HandleSpellResolved;
    }
    private void HandleSpellResolved()
    {
        if (currentSpellCircle != null)
            currentSpellCircle.OnSpellResolved -= HandleSpellResolved;
        OnActionResolved?.Invoke();
    }
}
