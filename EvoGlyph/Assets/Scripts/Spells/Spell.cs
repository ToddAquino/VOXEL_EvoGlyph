using System.Collections.Generic;
using UnityEngine;
using System;

public enum SpellType
{
    Projectile,
    Instant
}
public class Spell : MonoBehaviour
{
    public event Action<Spell> OnSpellDespawn;
    public List<SpellEffect> effects;
    public GameObject spellTarget;
    public SpellType spellType;
    public Animator animator;
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        animator.Rebind();
        animator.Update(0f);
    }

    public void Initialize(GameObject target)
    {
        spellTarget = target;


        animator.ResetTrigger("OnImpact");
        animator.ResetTrigger("OnCastFinish");

        animator.SetBool("IsProjectile", spellType == SpellType.Projectile);
        animator.Play("OnCast",0,0);
        col.enabled = false;
    }

    public void Deinitialize()
    {
        spellTarget = null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject objHit = collision.gameObject;
        if (objHit == spellTarget)
        {
            animator.SetTrigger("OnImpact");
            TriggerSpellEffects(objHit);
            Debug.Log("Spell HIT");
        }
    }

    public void OnCastFinished()
    {
        col.enabled = true;
        animator.SetTrigger("OnCastFinish");
    }
    public void TriggerSpellEffects(GameObject target)
    {
        if (target == null) return;

        foreach (var effect in effects)
            effect.Apply(target);
    }
    public void DespawnSpell()
    {
        OnSpellDespawn?.Invoke(this);
        Deinitialize();
        SpellSpawner.ReturnObjectToPool(this.gameObject);
    }
}