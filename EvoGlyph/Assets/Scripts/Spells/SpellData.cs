using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Spell Data")]
public class SpellData : ScriptableObject
{
    public GameObject MagicCirclePrefab;
    public GameObject SpellPrefab;
    public GameObject ControllerPrefab;
    public ElementType ElementType;
    public TargetType TargetType;
    public string audioID;

    //public SpellCircle BeginCasting(Unit user)
    //{
    //    var SpellCircleObj = Instantiate(MagicCirclePrefab, user.transform.position, user.transform.rotation);
    //    if (audioID != null)
    //    {
    //        AudioManager.Instance.PlaySFX(audioID);
    //    }
    //    SpellCircle spellCircle = SpellCircleObj.GetComponent<SpellCircle>();
    //    return spellCircle;
    //}
}

public enum TargetType
{
    Enemy,
    Self
}