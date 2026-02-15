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
    public string audioID;
    public Spell Activate(Unit user)
    {
        TargetingController targeting = user.TargetingController;
        TargetType targetType = GlyphData.spellPrefab.GetComponent<Spell>().targetType;
        targeting.BeginTargetSelection(targetType);

        if (targeting.currentValidTargets == null || targeting.currentValidTargets.Count == 0)
            return null;

        //For Now Picks first available target (Can edit targeting controller to enable changing targets)
        Unit target = targeting.currentValidTargets[0];

        if (target == null) return null;

        HealthComponent health = target.GetComponent<HealthComponent>();
        if (health == null || !health.IsAlive) return null;

        if (targeting.currentValidTargets[0] == null || !targeting.currentValidTargets[0].GetComponent<HealthComponent>().IsAlive) return null;
        
        targeting.SelectTarget(target);
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
        if (audioID != null) 
        { 
            AudioManager.Instance.PlaySFX(audioID);
        }
        var SpellObj = SpellSpawner.Instance.CreateSpellPrefab(GlyphData.spellPrefab,target.transform.position,target.transform.rotation);
        SpellObj.Initialize(target);
        Unit target = user.GetTarget();
        if (target == null || !target.GetComponent<HealthComponent>().IsAlive) return null;

        GameObject targetObj = target.gameObject;
        var SpellObj = SpellSpawner.Instance.CreateSpellPrefab(GlyphData.spellPrefab, 
            targetObj.transform.position, targetObj.transform.rotation);

        SpellObj.Initialize(targetObj);
        Spell = SpellObj.GetComponent<Spell>();
        return Spell;
    }
}