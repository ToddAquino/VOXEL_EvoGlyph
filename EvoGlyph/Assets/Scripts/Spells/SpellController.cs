using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    public event Action OnSpellResolved;

    Unit caster;

    SpellData spellData;
    SpellCircle spellCircle;
    Spell projectile;

    [Header("Spell Settings")]
    public List<SpellEffect> SpellEffects;
    public ElementType ElementType;
    [SerializeField] Unit target;
    [SerializeField] float damageMultiplier = 1.0f;
    public bool IsInterrupted = false;
    int effectsFinished;
    public void Initialize(Unit Caster, SpellData Data)
    {
        caster = Caster;
        spellData = Data;
        ElementType = spellData.ElementType;
        TargetType targetType = spellData.TargetType;

        if (targetType == TargetType.Self)
            target = caster;

        else if (targetType == TargetType.Enemy && caster.GetTarget() != null)
            target = caster.GetTarget();

        BeginCasting(Caster);
    }

    public void BeginCasting(Unit user)
    {
        GameObject MagicCirclePrefab = spellData.MagicCirclePrefab;
        string audioID = spellData.audioID;

        var SpellCircleObj = Instantiate(MagicCirclePrefab, user.transform.position, user.transform.rotation);
        if (!string.IsNullOrEmpty(audioID))
        {
            AudioManager.Instance.PlaySFX(audioID);
        }
        spellCircle = SpellCircleObj.GetComponent<SpellCircle>();
        spellCircle.OnCastFinished += HandleSpawnSpell;
        spellCircle.PerformCast(caster);
    }

    public void HandleSpawnSpell()
    {
        spellCircle.OnCastFinished -= HandleSpawnSpell;
        GameObject spellPrefabObj = spellData.SpellPrefab;

        GameObject spellObj = Instantiate(spellPrefabObj, caster.transform.position, caster.transform.rotation);
        projectile = spellObj.GetComponent<Spell>();

        projectile.Initialize(target);
        projectile.OnHit += OnProjectileHit;
    }

    public void OnProjectileHit(GameObject target)
    {
        projectile.OnHit -= OnProjectileHit;
        Destroy(projectile.gameObject, 1f);
        TriggerSpellEffects(target);
    }
    public void TriggerSpellEffects(GameObject target)
    {
        if (target == null) return;

        effectsFinished = 0;
        int totalEffects = SpellEffects.Count;

        foreach (var effect in SpellEffects)
        {
            effect.OnEffectFinished += HandleEffectFinished;
            effect.Apply(target,this);
        }
    }
    private void HandleEffectFinished(SpellEffect effect)
    {
        effect.OnEffectFinished -= HandleEffectFinished;
        int totalEffects = SpellEffects.Count;
        effectsFinished++;

        if (effectsFinished >= totalEffects)
        {

            HandleSpellFinished();
        }
    }
    public void SpellInterrupted()
    {
        IsInterrupted = true;

        if (projectile != null)
        {
            projectile.OnHit -= OnProjectileHit;
            Destroy(projectile.gameObject, 0.5f);
        }

        if (spellCircle != null)
        {
            spellCircle.OnInterruptFinished += HandleInterruptFinished;
            spellCircle.Animator.SetTrigger("OnInterrupted");
        }
    }
    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }

    public void SpellDeflected()
    {
        target = caster;
    }
    private void HandleInterruptFinished()
    {
        if (spellCircle != null)
            spellCircle.OnInterruptFinished -= HandleInterruptFinished;

        HandleSpellFinished();
    }
    public void HandleSpellFinished()
    {
        OnSpellResolved?.Invoke();
        spellCircle.Animator.SetTrigger("OnFinished");
        Destroy(spellCircle.gameObject, 1f);
        Destroy(gameObject,1f);
    }
}
