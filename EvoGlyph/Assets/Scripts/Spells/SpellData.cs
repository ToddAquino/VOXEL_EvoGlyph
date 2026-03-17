using System.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellDefinition", menuName = "Spells/Spell Definiton")]
public class SpellData : ScriptableObject
{
    public GameObject MagicCirclePrefab;
    public string audioID;

    public SpellCircle BeginCasting(Unit user)
    {
        var SpellCircleObj = Instantiate(MagicCirclePrefab, user.transform.position, user.transform.rotation);
        if (audioID != null)
        {
            AudioManager.Instance.PlaySFX(audioID);
        }
        SpellCircle spellCircle = SpellCircleObj.GetComponent<SpellCircle>();
        return spellCircle;
    }
}

