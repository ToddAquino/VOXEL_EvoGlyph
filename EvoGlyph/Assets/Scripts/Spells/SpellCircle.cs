using System;
using UnityEngine;

public class SpellCircle : MonoBehaviour
{
    public event Action OnCastFinished;
    public event Action OnInterruptFinished;
    public Animator Animator;
    //[SerializeField] GameObject spellProjectileObj;
    //GameObject Caster;
    //GameObject currentTarget;



    //SpellProjectile projectile;
    public void PerformCast(Unit caster)
    {
        Animator.Play("On Cast Start", 0, 0);
      
    }

    //void Deinitialize()
    //{
    //    //IsInterrupted = false;
    //    //projectile.OnSpellDespawn -= HandleSpellFinished;
    //    Destroy(gameObject, 1f);
    //}

    public void OnCastAnimationFinished()
    {
        OnCastFinished?.Invoke();
        //if (IsInterrupted)
        //{
        //    Debug.Log("Spell Interrupted!");
        //    HandleSpellFinished();
        //    return;
        //}
        //GameObject spellObj = Instantiate(spellProjectileObj, transform.position, transform.rotation);
        //projectile = spellObj.GetComponent<SpellProjectile>();

        //projectile.Initialize(currentTarget, damageMultiplier);
        //projectile.OnSpellDespawn += HandleSpellFinished;

    }
    public void OnInterruptedAnimationFinished()
    {
        OnInterruptFinished?.Invoke();
    }
    //public void HandleSpellFinished()
    //{
    //    OnSpellResolved?.Invoke();
    //    animator.SetTrigger("OnFinished");
    //    DespawnCircle();
    //}
    //public void SpellInterrupted()
    //{
    //    IsInterrupted = true;
    //    animator.SetTrigger("OnInterrupted");
    //}
    //public void SetDamageMultiplier(float multiplier)
    //{
    //    damageMultiplier = multiplier;
    //}
    //public void SpellDeflected()
    //{
    //    currentTarget = Caster;
    //}

    //void DespawnCircle()
    //{
    //    Destroy(gameObject, 1f);
    //}
}
