using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Element/Element Data")]
public class ElementData : ScriptableObject
{
    public Sprite Icon;
    public ElementType Type;
    public List<ElementType> ElementImmunity;
    public List<ElementType> ElementWeakness;
    public List<ElementType> ElementResistance;
}
