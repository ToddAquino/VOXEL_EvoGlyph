using System.Collections.Generic;
using UnityEngine;

public class ElementalSynergyDatabase : MonoBehaviour
{
    [Header("Glyph Library")]
    public List<ElementalSynergy> Synergies;
    public ElementalSynergy GetMatchingSynergy(List<StatusEffectData> activeStatusEffects, StatusEffectData appliedStatus)
    {
        foreach (var synergy in Synergies)
        {
            bool matches =
            (synergy.A == appliedStatus && activeStatusEffects.Contains(synergy.B)) ||
            (synergy.B == appliedStatus && activeStatusEffects.Contains(synergy.A));

            if (matches)
            {
                return synergy;
            }
        }
        return null;
    }
}
[System.Serializable]
public class ElementalSynergy
{
    public StatusEffectData A;
    public StatusEffectData B;
    public StatusEffectData Result;
}