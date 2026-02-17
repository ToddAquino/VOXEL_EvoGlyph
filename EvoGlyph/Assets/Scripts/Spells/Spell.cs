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
    SpellType currentSpellType;
    public TargetType targetType;
    TargetType currentTargetType;
    public Animator animator;
    public Collider2D col;
    public float projectileSpeed = 5f;
    bool isMoving;
    private int effectsFinished;
    private int totalEffects;
    private Vector3 projectileDirection;
    public float damageMultiplier = 1.0f;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        animator.Rebind();
        animator.Update(0f);
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position += projectileDirection * projectileSpeed * Time.deltaTime;
        }
    }

    public void Initialize(GameObject target)
    {
        currentSpellType = spellType;
        currentTargetType = targetType;
        damageMultiplier = 1.0f;
        spellTarget = target;
        Unit unit = spellTarget.GetComponent<Unit>();
        if (unit != null)
        {
            if (unit.Team == Team.Player)
            {
                projectileDirection = Vector3.left;

                if (targetType != TargetType.Self)
                {
                    if (this.transform.localScale.x != -1)
                    {
                        this.transform.localScale = new Vector3(-1, this.transform.localScale.y, this.transform.localScale.z);
                    }
                }
            }
            else if (unit.Team == Team.Enemy && targetType != TargetType.Self)
            {
                projectileDirection = Vector3.right;

                if (targetType != TargetType.Self)
                {
                    if (this.transform.localScale.x != 1)
                    {
                        this.transform.localScale = new Vector3(1, this.transform.localScale.y, this.transform.localScale.z);
                    }
                }            
            }
        }

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
            isMoving = false;
            transform.position = spellTarget.transform.position;
            //animator.SetTrigger("OnImpact");
            TriggerSpellEffects(objHit);
            Debug.Log("Spell HIT");
        }
    }

    public void OnCastFinished()
    {
        col.enabled = true;
        animator.SetTrigger("OnCastFinish");
    }

    public void HandleProjectileMovement()
    {
        if (currentSpellType == SpellType.Projectile)
        {
            isMoving = true;
        }
        else if (currentSpellType == SpellType.Instant)
        {
            transform.position = spellTarget.transform.position;
        }
    }
    public void TriggerSpellEffects(GameObject target)
    {
        if (target == null) return;

        effectsFinished = 0;
        totalEffects = effects.Count;

        foreach (var effect in effects)
        {
            effect.OnEffectFinished += HandleEffectFinished;
            effect.Apply(target);
        }
    }
    private void HandleEffectFinished(SpellEffect effect)
    {
        effect.OnEffectFinished -= HandleEffectFinished;

        effectsFinished++;

        if (effectsFinished >= totalEffects)
        {
            animator.SetTrigger("OnImpact");
            //DespawnSpell();
        }
    }
    public void DespawnSpell()
    {
        Debug.Log("Spell Despawned");
        OnSpellDespawn?.Invoke(this);
        Deinitialize();
        SpellSpawner.ReturnObjectToPool(this.gameObject);
    }
    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    public void OverrideTarget(TargetType newTargetType, SpellType newSpellType, GameObject target)
    {
        spellTarget = target;
        currentTargetType = newTargetType;
        currentSpellType = newSpellType;
        animator.SetBool("IsProjectile", currentSpellType == SpellType.Projectile);
        HandleProjectileMovement();
    }
}