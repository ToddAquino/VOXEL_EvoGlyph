using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public enum Team
{
    Player,
    Enemy
}
public class Unit : MonoBehaviour
{ 
    [Header("Stats")]
    public HealthComponent HealthComponent;
    public Team Team;

    public Vector3 SpawnPosition;
    [Header("Target")]
    public Unit SelectedTarget;
    [Header("Visual")]
    public Animator animator;
    public SpriteRenderer characterSprite;
    public Transform spellSpawnAnchor;
    public bool hasDied = false;
    public IUnitController Controller;
    public virtual void Initialize()
    {
        Controller = GetComponent<IUnitController>();
        HealthComponent.InitializeHealth();
        HealthComponent.OnDeath.AddListener(OnDeath);
        HealthComponent.OnHit.AddListener(OnHit);
        gameObject.SetActive(true);
    }

    public virtual void Deinitialize()
    {
        HealthComponent.OnDeath.RemoveListener(OnDeath);
        gameObject.SetActive(false);
    }
    public void StartTurn(BattlePhase phase)
    {
        TickStatusEffects();
        Controller?.OnStartTurn();
    }
    public void EndTurn(BattlePhase phase)
    {
        Controller?.OnEndTurn();
    }
    public void SetTarget(Unit Target)
    {
        SelectedTarget = Target;
    }

    public Unit GetTarget()
    {
        return SelectedTarget;
    }

    public void MoveToSpawnPosition()
    {
        this.transform.position = SpawnPosition;
    }

    public void MoveToTargetPosition()
    {
        this.transform.position = SelectedTarget.transform.position;
    }
    private void TickStatusEffects()
    {
        StatusEffectComponent statusComp = GetComponent<StatusEffectComponent>();
        if (statusComp == null || statusComp.ActiveStatuses.Count == 0) return;

        // Iterate backwards so RemoveStatus calls don't skip entries
        for (int i = statusComp.ActiveStatuses.Count - 1; i >= 0; i--)
        {
            StatusEffectData status = statusComp.ActiveStatuses[i];

            status.TurnsRemaining--;
            Debug.Log($"[Status] {status.Effect} on {name}: {status.TurnsRemaining} turn(s) left.");

            if (status.TurnsRemaining <= 0)
            {
                Debug.Log($"[Status] {status.Effect} expired on {name}.");
                statusComp.RemoveStatus(status);
            }
        }
    }
    public virtual void CheckConditions()
    {
        StatusEffectComponent statusComp = GetComponent<StatusEffectComponent>();

        if (statusComp == null || statusComp.ActiveStatuses.Count == 0)
        {
            Debug.Log("No status on " + Team);
            return;
        }
        List<StatusEffectData> statuses = statusComp.ActiveStatuses;

        bool hasWet = HasStatus(statusComp, StatusEffect.Wet);
        bool hasBurning = HasStatus(statusComp, StatusEffect.Burning);
        bool hasShocked = HasStatus(statusComp, StatusEffect.Shocked);
        bool hasElectrocute = HasStatus(statusComp, StatusEffect.Electrocute);
        //bool hasArcane = HasStatus(statusComp, StatusEffect.Arcane);
        if (hasBurning)
        {
            int burnDamage = Mathf.RoundToInt(5); //HARDCODED DMG, CHANGE IF NEEDED
            HealthComponent.TakeDamage(burnDamage);

            // Burning is extinguished by Wet
            if (hasWet)
            {
                RemoveStatus(statusComp, StatusEffect.Burning);
            }
            AudioManager.Instance.PlaySFX("fire1", 0.8f);
            //Debug.Log("FIRE");
        }
        if (hasElectrocute)
        {
            // Consume Wet for bonus damage
            int shockBonus = Mathf.RoundToInt(10);
            HealthComponent.TakeDamage(shockBonus);
            AudioManager.Instance.PlaySFX("electric1", 0.8f);
            RemoveStatus(statusComp, StatusEffect.Electrocute);
        }
        if (hasWet)
        {
            int waterDamage = Mathf.RoundToInt(2);
            HealthComponent.TakeDamage(waterDamage);
            Debug.Log("WET");
            // Wet is removed by Burning
            AudioManager.Instance.PlaySFX("water1", 0.8f);
            if (hasBurning)
            {
                Debug.Log("Wet removed!");
                RemoveStatus(statusComp, StatusEffect.Wet);
            }
        }
    }
    public bool HasStatus(StatusEffectComponent comp, StatusEffect effect)
    {
        foreach (var status in comp.ActiveStatuses)
        {
            if (status.Effect == effect)
                return true;
        }
        return false;
    }

    public void RemoveStatus(StatusEffectComponent comp, StatusEffect effect)
    {
        for (int i = comp.ActiveStatuses.Count - 1; i >= 0; i--)
        {
            if (comp.ActiveStatuses[i].Effect == effect)
            {
                comp.RemoveStatus(comp.ActiveStatuses[i]);
            }
        }
    }
    public virtual void OnDeath()
    {
        if (hasDied) return;
        if (animator != null)
            animator.SetTrigger("OnDeath");
        
        hasDied = true;
        Debug.Log("Unit Died");
        AudioManager.Instance.PlaySFX("defeated", 0.7f);
        BattleManager.Instance?.OnUnitDied(this);
        HealthComponent.HideHealthBar();
    }
    public void OnHit()
    {
        if (animator != null)
            animator.SetTrigger("OnHit");
    }
}
public interface IUnitController
{
    void OnStartTurn();
    void OnEndTurn();
}