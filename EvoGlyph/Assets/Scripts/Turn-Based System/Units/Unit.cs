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