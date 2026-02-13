using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BattleState
{
    Waiting,
    PlayerTurn,
    OpponentTurn,
    Won,
    Lost
}
public class BattleController : MonoBehaviour
{
    public BattleState state { get; private set; }
    int TurnIndex = 0;
    public Unit currentActiveUnit;
    bool battleEnded = false;
    [Header("UI")]
    [SerializeField] BattleInformation BattleSystemUI;
    public List<Unit> aliveUnits = new List<Unit>();
    public void Initialize()
    {
        //adding for multiple rounds
        if (currentActiveUnit != null)
        {
            currentActiveUnit.EndTurn();
        }
        DeInitialize();
        currentActiveUnit = null;
        state = BattleState.Waiting;
        TurnIndex = 0;
        battleEnded = false;
    }

    public void DeInitialize()
    {
        currentActiveUnit = null;
        TurnIndex = 0;
        battleEnded = false;
    }

    public void StartNextTurn()
    {
        if (currentActiveUnit == BattleManager.Instance.playerUnit)
        {
            state = BattleState.PlayerTurn;
        }
        else if (BattleManager.Instance.enemyUnits.Any(enemy => enemy == currentActiveUnit))
        {
            state = BattleState.OpponentTurn;
        }
        Debug.Log($"<color=lime> {currentActiveUnit.name}'s Turn</color>");
        BattleSystemUI.UpdateText(state);
        currentActiveUnit.StartTurn();
    }

    public void ChangeNextActiveUnit()
    {
        CheckEndCondition();
        if (battleEnded) return;
        if (aliveUnits.Count == 0) return;
        TurnIndex %= aliveUnits.Count;
        currentActiveUnit = aliveUnits[TurnIndex];
        StartNextTurn();
    }
    public void UnitEndedItsTurn()
    {
        if (battleEnded) return;
        if (state == BattleState.Won || state == BattleState.Lost) return;
        state = BattleState.Waiting;
        TurnIndex++;
        ChangeNextActiveUnit();
    }
    public void CheckEndCondition()
    {
        if (!aliveUnits.Contains(BattleManager.Instance.playerUnit))
        {
            state = BattleState.Lost;
            battleEnded = true;

        }
        else if (!BattleManager.Instance.enemyUnits.Any(unit => aliveUnits.Contains(unit)))
        {
            state = BattleState.Won;
            battleEnded = true;
        }
        BattleSystemUI.UpdateText(state);
        if (state == BattleState.Lost || state == BattleState.Won)
        {
            BattleManager.Instance.EndBattle();
            state = BattleState.Waiting;
        }
    }
    public void OnUnitRemoved(Unit unit)
    {
        int removedUnitIndex = aliveUnits.IndexOf(unit);

        if (removedUnitIndex < TurnIndex)
        {
            TurnIndex--;
        }
        aliveUnits.Remove(unit);
        CheckEndCondition();
        if (battleEnded && currentActiveUnit == BattleManager.Instance.playerUnit)
        {
            currentActiveUnit.EndTurn();
        }
    }
}
