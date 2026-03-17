using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Element/Element Data")]
public class ElementData : ScriptableObject
{
    public ElementType ElementType;
    public List<ElementType> ElementImmunity;
    public List<ElementType> ElementWeakness;
    public List<ElementType> ElementResistance;
}
