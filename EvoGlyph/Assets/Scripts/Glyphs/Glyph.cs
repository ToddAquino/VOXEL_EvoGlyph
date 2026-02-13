using System;
using UnityEngine;
using UnityEngine.UI;
public class Glyph : MonoBehaviour
{
    public bool IsActivated = false;
    public bool IsCastToSelf;
    public GlyphData GlyphData;
    public Sprite ItemIcon;
    public Spell Spell;
    public Spell Activate(Unit user)
    {
        //if (IsActivated)
        //{
        //    Debug.Log($"Glyph: {GlyphData.name} Already Activated");
        //    return;
        //}
        //IsActivated = true;
        return CastSpell(user);
    }

    public Spell CastSpell(Unit user)
    {
        GameObject target = null;
        if (IsCastToSelf)
        {
            target = user.gameObject;
        }
        else
        {
            target = user.targetEnemy.gameObject;
        }

        var SpellObj = SpellSpawner.Instance.CreateSpellPrefab(GlyphData.spellPrefab,target.transform.position,target.transform.rotation);
        SpellObj.Initialize(target);
        Spell = SpellObj.GetComponent<Spell>();
        return Spell;
    }
}