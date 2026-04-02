using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectData", menuName = "Spells/StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    public StatusEffect Effect;
    public Sprite Icon;
    public bool isDerived;
}

public enum StatusEffect
{
    None,
    Wet,
    Burning,
    Shocked,
    Electrocute, //Wet + Shock
    Arcane
}