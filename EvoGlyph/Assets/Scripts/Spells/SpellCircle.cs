using System;
using UnityEngine;

public class SpellCircle : MonoBehaviour
{
    public event Action OnSpellResolved;
    [SerializeField] Animator animator;
    [SerializeField] GameObject spellProjectileObj;
    GameObject Caster;
    GameObject currentTarget;
    public bool IsInterrupted = false;
    [SerializeField]TargetType targetType;
    float damageMultiplier = 1.0f;
    SpellProjectile projectile;
    public void PerformCast(Unit caster)
    {
        animator.Play("On Cast Start", 0, 0);
        Caster = caster.gameObject;
        if(targetType == TargetType.Self)
            currentTarget = caster.gameObject;
        else
            if (caster.GetTarget() != null)
            currentTarget = caster.GetTarget().gameObject;
    }

    void Deinitialize()
    {
        IsInterrupted = false;
        projectile.OnSpellDespawn -= HandleSpellFinished;
        Destroy(gameObject, 1f);
    }

    public void OnCastAnimationFinished()
    {
        if (IsInterrupted)
        {
            Debug.Log("Spell Interrupted!");
            HandleSpellFinished();
            return;
        }
        GameObject spellObj = Instantiate(spellProjectileObj, transform.position, transform.rotation);
        projectile = spellObj.GetComponent<SpellProjectile>();

        projectile.Initialize(currentTarget, damageMultiplier);
        projectile.OnSpellDespawn += HandleSpellFinished;

    }

    public void HandleSpellFinished()
    {
        OnSpellResolved?.Invoke();
        animator.SetTrigger("OnFinished");
        DespawnCircle();
    }
    public void SpellInterrupted()
    {
        IsInterrupted = true;
        animator.SetTrigger("OnInterrupted");
    }
    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }
    public void SpellDeflected()
    {
        currentTarget = Caster;
    }

    void DespawnCircle()
    {
        Destroy(gameObject, 1f);
    }
}
