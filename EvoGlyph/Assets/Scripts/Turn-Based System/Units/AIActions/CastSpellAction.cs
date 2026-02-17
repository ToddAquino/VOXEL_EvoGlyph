using System.Collections;
using UnityEngine;

public class CastSpellAction : MonoBehaviour
{
    public Glyph glyphToCast;
    public bool IsCastingFinished;
    public bool IsSpellResolved;
    QuickTimeEventResult qteResult;

    Spell spawnedSpell;
    //private bool spellFinished;
    public void Activate(Unit user)
    {
        //spawnedSpell = null;
        //base.Activate(user);
        IsCastingFinished = false;
        IsSpellResolved = false;
        //user.StartCoroutine(PerformAction(user));
    }
    public void SetQTEResult(QuickTimeEventResult result)
    {
        qteResult = result;
    }
    public void ReleaseSpell(Unit user)
    {
        IsCastingFinished = true;

        spawnedSpell = glyphToCast.Activate(user);

        if (spawnedSpell == null)
        {
            IsSpellResolved = true;
            return;
        }

        spawnedSpell.OnSpellDespawn += OnSpellFinished;

        switch (qteResult)
        {
            case QuickTimeEventResult.Success:
                spawnedSpell.SetDamageMultiplier(0f);
                break;

            case QuickTimeEventResult.Perfect:
                spawnedSpell.SetDamageMultiplier(0.25f);
                spawnedSpell.OverrideTarget(user.gameObject);
                break;

            case QuickTimeEventResult.Failed:
                spawnedSpell.SetDamageMultiplier(1f);
                break;
        }
    }

    //private IEnumerator PerformAction(Unit user)
    //{
    //    spellFinished = false;
    //    spawnedSpell = glyphToCast.Activate(user);
    //    //yield return new WaitForSeconds(1f);
    //    if (spawnedSpell != null)
    //    {
    //        spawnedSpell.OnSpellDespawn += OnSpellFinished;
    //        yield return new WaitUntil(() => spellFinished);
    //    }

    //    ActionSuccessfullyExecuted();
    //    user.EndTurn(BattlePhase.EnemyAction);
    //}

    private void OnSpellFinished(Spell spell)
    {
        spell.OnSpellDespawn -= OnSpellFinished;
        IsSpellResolved = true;
    }
}
