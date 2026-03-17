using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    public event Action OnSpellDespawn;
    [SerializeField] Animator animator;
    [SerializeField] float projectileSpeed = 5f;
    bool isMoving;

    GameObject spellTarget;
    public float damageMultiplier;
    [SerializeField] List<SpellEffect> effects;
    private int effectsFinished;
    Vector3 projectileDirection;
    public void Initialize(GameObject target, float multiplier)
    {
        spellTarget = target;
        damageMultiplier = multiplier;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (target.transform.position.x < this.transform.position.x)
        {
            projectileDirection = Vector3.left;
            spriteRenderer.flipX = true;
        }
        else 
        {
            projectileDirection = Vector3.right;
            spriteRenderer.flipX = false;
        }
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position += projectileDirection * projectileSpeed * Time.deltaTime;
        }
    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject objHit = collision.gameObject;
        if (objHit == spellTarget)
        {
            isMoving = false;
            transform.position = spellTarget.transform.position;
            TriggerSpellEffects(objHit);
            Debug.Log("Spell Hit");
        }
    }
    public void TriggerSpellEffects(GameObject target)
    {
        if (target == null) return;

        effectsFinished = 0;
        int totalEffects = effects.Count;

        foreach (var effect in effects)
        {
            effect.OnEffectFinished += HandleEffectFinished;
            effect.Apply(target);
        }
    }
    private void HandleEffectFinished(SpellEffect effect)
    {
        effect.OnEffectFinished -= HandleEffectFinished;
        int totalEffects = effects.Count;
        effectsFinished++;

        if (effectsFinished >= totalEffects)
        {
            animator.SetTrigger("OnImpact");
            DespawnSpell();
        }
    }
    public void DespawnSpell()
    {
        StartCoroutine(DoDespawn());
    }

    IEnumerator DoDespawn()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Spell Despawned");
        OnSpellDespawn?.Invoke();
        //SpellSpawner.ReturnObjectToPool(this.gameObject);
        Destroy(gameObject, 1f);
    }
}
