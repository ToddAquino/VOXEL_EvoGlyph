using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StatusEffectComponent : MonoBehaviour
{
    public List<BaseStatus> ActiveBaseStatusEffects = new List<BaseStatus>();
    public List<DerivedStatus> ActiveDerivedStatusEffects = new List<DerivedStatus>();
    public void ApplyStatusElement(ElementType element)
    {
        BaseStatus type;
        switch (element)
        {
            case ElementType.Fire:
                type = BaseStatus.Burning;
                break;
            case ElementType.Water:
                type = BaseStatus.Wet;
                break;
            case ElementType.Lightning:
                type = BaseStatus.Shocked;
                break;
            default:
                type = BaseStatus.None;
                break;
        }

        if (type == BaseStatus.None || ActiveBaseStatusEffects.Contains(type))
        {
            Debug.Log("No Status Effect Added");
            return;
        }
        else
        {
            ApplyBaseStatusEffect(type);
        }
        HandleStatusSynergies(type);
    }

    //Base Status Element Effects
    public void ApplyBaseStatusEffect(BaseStatus type)
    {
        ActiveBaseStatusEffects.Add(type);
    }
    public void RemoveBaseStatusEffect(BaseStatus type)
    {
        ActiveBaseStatusEffects.Remove(type);
    }

    //Derived Status Element Effects
    public void ApplyDerivedStatusEffect(BaseStatus type)
    {
        ActiveBaseStatusEffects.Add(type);
    }
    public void RemoveDerivedStatusEffecct(BaseStatus type)
    {
        ActiveBaseStatusEffects.Remove(type);
    }

    //Handle Combinations of Base Status Elements to form Derived Status Effect
    private void HandleStatusSynergies(BaseStatus appliedElement)
    {
        DerivedStatus newStatus = GameManager.Instance.ElementalSynergyDatabase.TryGetSynergyResult(appliedElement, ActiveBaseStatusEffects);
        if (newStatus != DerivedStatus.None)
        {
            ActiveDerivedStatusEffects.Add(newStatus);
            RemoveBaseStatusesForSynergy(appliedElement, newStatus);
        }
    }

    private void RemoveBaseStatusesForSynergy(BaseStatus appliedElement, DerivedStatus derived)
    {
        ElementalSynergy synergy = GameManager.Instance.ElementalSynergyDatabase.Synergies.Find(s => s.Result == derived &&(s.A == appliedElement || s.B == appliedElement));

        if (synergy == null) return;

        ActiveBaseStatusEffects.Remove(synergy.A);
        ActiveBaseStatusEffects.Remove(synergy.B);
    }
}

