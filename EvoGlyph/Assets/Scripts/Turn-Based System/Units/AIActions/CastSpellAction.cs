using System;
using System.Collections;
using UnityEngine;

public class CastSpellAction : MonoBehaviour
{
    public event Action OnActionResolved; 
    //public SpellData spellToCast;
    private SpellController currentController;

    QuickTimeEventResult qteResult;
    public void SetQTEResult(QuickTimeEventResult result)
    {
        qteResult = result;
    }
    public void ReleaseSpell(Unit user)
    {
        SpellData spellToCast = user.GetComponent<EnemyUnit>().spellToCast;
        if (spellToCast != null)
        {
            currentController = Instantiate(spellToCast.ControllerPrefab).GetComponent<SpellController>();
            currentController.OnSpellResolved += HandleSpellResolved;
            currentController.Initialize(user, spellToCast);
            ApplyQTEResult();
        }
    }
    private void ApplyQTEResult()
    {
        switch (qteResult)
        {
            case QuickTimeEventResult.Success:
                currentController.SpellInterrupted();
                break;

            case QuickTimeEventResult.Perfect:
                currentController.SetDamageMultiplier(0.25f);
                currentController.SpellDeflected();
                break;

            default:
                currentController.SetDamageMultiplier(1f);
                break;
        }
    }
    private void HandleSpellResolved()
    {
        if (currentController != null)
            currentController.OnSpellResolved -= HandleSpellResolved;
        OnActionResolved?.Invoke();
    }
}
