using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BattleState
{
    Setup,
    Battle,
    Won,
    Lost
}
public class TurnBaseSystem : MonoBehaviour
{
    public static TurnBaseSystem Instance;
    public BattleState state;
    public List<Unit> UnitList = new List<Unit>();
    List<Unit> activeUnits = new List<Unit>();
    List<int> aliveTeams = new List<int>();
    public int TurnIndex = 0;
    public Unit currentActiveUnit;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        state = BattleState.Setup;
        SetupBattle();
    }

    public void SetupBattle()
    {
        activeUnits = UnitList.OrderByDescending(u => u.GetSpeed()).ToList();
        Debug.Log($"<color=cyan>List: {string.Join(",", activeUnits)}</color>");

        state = BattleState.Battle;
        BeginBattle();
    }

    public void BeginBattle()
    {
        TurnIndex = 0;
        StartNextTurn();
    }
    public void StartNextTurn()
    {
        if (activeUnits.Count == 0)
            return;

        currentActiveUnit = activeUnits[TurnIndex % activeUnits.Count];
        Debug.Log($"<color=lime> {currentActiveUnit.GetName()}'s Turn</color>");
        currentActiveUnit.StartTurn();
    }
    public void EndTurn()
    {
        TurnIndex++;
        StartNextTurn();
    }

    public void CheckEndConditions()
    {
        // Remove dead units in pool
        activeUnits.RemoveAll(d => d.CurrentHP <= 0);

        if (GetAliveTeams() <= 1)
        {
            EndTurnBaseSystem();
        }
    }

    private int GetAliveTeams()
    {

        foreach (var unit in activeUnits)
        {
            if (unit.GetTeamNumber() != 0 && !aliveTeams.Contains(unit.GetTeamNumber()))
            {
                aliveTeams.Add(unit.GetTeamNumber());
            }
        }

        return aliveTeams.Count;
    }

    private void EndTurnBaseSystem() // Ends Turn base battle
    {
        if(aliveTeams.Contains(0))
        {
            state = BattleState.Won;
        }
        else
        {
            state = BattleState.Lost;
        }
    }
}
