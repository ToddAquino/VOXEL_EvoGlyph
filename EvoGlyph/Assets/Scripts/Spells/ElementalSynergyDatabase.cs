using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    None,
    Arcane,
    Fire,
    Water,
    Lightning,
    Shield
}
public enum BaseStatus
{
    None,
    Wet, //Water
    Frozen,
    Burning, //Fire
    Shocked, //Lightning

}
public enum DerivedStatus
{
    None,
    Paralyzed
}
public class ElementalSynergyDatabase : MonoBehaviour
{
    [Header("Glyph Library")]
    public List<ElementalSynergy> Synergies;
    public DerivedStatus TryGetSynergyResult(BaseStatus appliedStatus, List<BaseStatus> activeStatusEffects)
    {
        foreach (var synergy in Synergies)
        {
            bool matches =
            (synergy.A == appliedStatus && activeStatusEffects.Contains(synergy.B)) ||
            (synergy.B == appliedStatus && activeStatusEffects.Contains(synergy.A));

            if (matches)
            {
                return synergy.Result;
            }
        }
        return DerivedStatus.None;
    }
}
[System.Serializable]
public class ElementalSynergy
{
    public BaseStatus A;
    public BaseStatus B;
    public DerivedStatus Result;
}