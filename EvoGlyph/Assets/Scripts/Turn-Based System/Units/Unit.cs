using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Stats")]
    public float Speed;
    public HealthComponent HealthComponent;

    public IUnitController Controller;
    public Unit targetEnemy;
    public virtual void Initialize()
    {
        Controller = GetComponent<IUnitController>();
        gameObject.SetActive(true);
        HealthComponent.InitializeHealth();
        HealthComponent.OnDeath.AddListener(OnDeath);
    }

    public virtual void Deinitialize()
    {
        HealthComponent.OnDeath.RemoveListener(OnDeath);
        HealthComponent.HideHealthBar();
        gameObject.SetActive(false);
    }
    public void StartTurn()
    {
        Controller?.OnStartTurn(this);
    }
    public void EndTurn()
    {
        Controller?.OnEndTurn(this);
        BattleManager.Instance.Controller.UnitEndedItsTurn();
    }
    public virtual void OnDeath()
    {
        BattleManager.Instance?.OnUnitDied(this);
        Deinitialize();
    }
    public void SetTarget(Unit unit)
    {
        targetEnemy = unit;
    }

    public Unit GetTarget()
    {
        return targetEnemy;
    }
}
public interface IUnitController
{
    void OnStartTurn(Unit unit);
    void OnEndTurn(Unit unit);
}