using UnityEngine;
using static EditorAttributesSamples.ExampleScriptableObject;

public class GreatMageAction : AIAction
{
    [SerializeField] SpellOption[] spellOptions;

    public SpellData spellToCast;
    private SpellController currentController;
    public override void DoAction(Unit user)
    {
        base.DoAction(user);
        ReleaseSpell(user);
    }

    public void ReleaseSpell(Unit user)
    {
        spellToCast = GetRandomSpellFromList();
        if (spellToCast != null)
        {
            currentController = Instantiate(spellToCast.ControllerPrefab).GetComponent<SpellController>();
            currentController.OnSpellResolved += HandleSpellResolved;
            currentController.Initialize(user, spellToCast);
            ApplyQTEResult();
        }
    }

    private SpellData GetRandomSpellFromList()
    {
        float totalChance = 0f;
        foreach (var spell in spellOptions)
        {
            totalChance += spell.Chance;
        }

        float randInt = Random.Range(0, totalChance);
        float current = 0f;

        foreach (var option in spellOptions)
        {
            current += option.Chance;

            if (randInt <= current)
            {
                
                return option.Spell;
            }
        }
        return null;
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
        HandleActionResolved();
    }
}

[System.Serializable]
public class SpellOption
{
    public SpellData Spell;
    public float Chance;
}
