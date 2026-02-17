using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public enum Team
{
    Player,
    Enemy
}
public class Unit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Action<Unit> OnTargetClicked;
    public Action<Unit> OnTargetHoverEnter;
    public Action<Unit> OnTargetHoverExit;

    [Header("Stats")]
    //public float Speed;
    public HealthComponent HealthComponent;

    public IUnitController Controller;
    public TargetingController TargetingController;
    public Team Team;
    public SpriteRenderer characterSprite;
    public virtual void Initialize()
    {
        TargetingController.Self = this;
        Controller = GetComponent<IUnitController>();
        gameObject.SetActive(true);
        HealthComponent.InitializeHealth();
        HealthComponent.OnDeath.AddListener(OnDeath);
    }

    public virtual void Deinitialize()
    {
        TargetingController.Self = null;
        HealthComponent.OnDeath.RemoveListener(OnDeath);
        HealthComponent.HideHealthBar();
        gameObject.SetActive(false);
    }
    public void StartTurn(BattlePhase phase)
    {
        Controller?.OnStartTurn();
    }
    public void EndTurn(BattlePhase phase)
    {
        Controller?.OnEndTurn();
        //BattleManager.Instance.Controller.UnitEndedItsTurn();
    }
    public virtual void OnDeath()
    {
        AudioManager.Instance.PlaySFX("defeated", 0.7f);
        BattleManager.Instance?.OnUnitDied(this);
        Deinitialize();
    }
    public void SetTarget(Unit Target)
    {
        TargetingController.SelectedTarget = Target;
    }
    public Unit GetTarget()
    {
        return TargetingController.SelectedTarget;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTargetClicked?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnTargetHoverEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnTargetHoverExit?.Invoke(this);
    }
}
public interface IUnitController
{
    void OnStartTurn();
    void OnEndTurn();
}