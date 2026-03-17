using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    None,
    Water,
    Lightning,
    Arcane,
    Fire,
    Ghost,
    Rock
}
public class ElementHandler : MonoBehaviour
{
    [SerializeField] private List<ElementData> ElementDataList;
    public float GetEffectiveness(ElementType attacker, ElementType defender)
    {
        if (ElementDataList == null || ElementDataList.Count == 0) return 1f;

        foreach (ElementData data in ElementDataList)
        {
            if (data.ElementType != defender)
                continue;

            if (data.ElementImmunity.Contains(attacker))
                return 0f;

            if (data.ElementWeakness.Contains(attacker))
                return 1.5f;

            if (data.ElementResistance.Contains(attacker))
                return 0.5f;
            return 1f;
        }

        return 1f;
    }
}
