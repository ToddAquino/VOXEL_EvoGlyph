using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TargetType
{
    SingleEnemy,
    Self,
    Ally
}
public class TargetingController : MonoBehaviour
{
    public Unit Self;
    public TargetType CurrentTargetType;


    [Header("Target Selection")]
    public Unit SelectedTarget;
    public List<Unit> currentValidTargets = new List<Unit>();
    public virtual void BeginTargetSelection(TargetType type)
    {
        CurrentTargetType = type;
        currentValidTargets = GetValidTargets(type);

        SelectedTarget = null;
       
    }

    public virtual void EndTargetSelection()
    {
        currentValidTargets.Clear();
    }

    public void SelectTarget(Unit targetSelected)
    {
        if (currentValidTargets.Contains(targetSelected))
        {
            SelectedTarget = targetSelected;
            EndTargetSelection();
        }
    }

   

    List<Unit> GetValidTargets(TargetType type)
    {
        if (BattleManager.Instance == null) return null;
        switch (type)
        {
            case TargetType.SingleEnemy:
                return BattleManager.Instance.Controller.aliveUnits
                    .Where(u => u.HealthComponent.IsAlive && u.Team != Self.Team).ToList();

            case TargetType.Self:
                return BattleManager.Instance.Controller.aliveUnits
                    .Where(u => u.HealthComponent.IsAlive && u == Self).ToList();

            case TargetType.Ally:
                return BattleManager.Instance.Controller.aliveUnits
                    .Where(u => u.HealthComponent.IsAlive && u.Team == Self.Team && u != Self).ToList();

            default:
                return null;
        }
    }
}
