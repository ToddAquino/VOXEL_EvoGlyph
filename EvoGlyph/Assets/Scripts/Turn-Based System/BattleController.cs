using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BattlePhase
{
    EnemyPlanning,
    PlayerCounter,
    EnemyAction,
    PlayerAction,
    Won,
    Lost
}
public class BattleController : MonoBehaviour
{
    public BattlePhase CurrentPhase { get; private set; }
    public List<Unit> aliveUnits = new List<Unit>();
    bool battleEnded = false;
    
    [Header("UI")]
    [SerializeField] BattleInformation BattleSystemUI;

    public void Initialize()
    {
        battleEnded = false;
        CurrentPhase = BattlePhase.EnemyPlanning;
        StartPhase();
    }
    public void StartPhase()
    {
        if (BattleSystemUI != null)
        {
            BattleSystemUI.UpdateText(CurrentPhase);
        }

        if (battleEnded) return;
        switch (CurrentPhase)
        {
            case BattlePhase.EnemyPlanning:
                StartCoroutine(StartEnemyPlanning());
                break;

            case BattlePhase.PlayerCounter:
                StartPlayerCounter();
                break;

            case BattlePhase.EnemyAction:
                StartCoroutine(StartEnemyAction());
                break;

            case BattlePhase.PlayerAction:
                StartPlayerAction();
                break;
        }
    }

    IEnumerator StartEnemyPlanning()
    {
        var enemies = aliveUnits.Where(u => u.Team == Team.Enemy).ToList();

        foreach (var enemy in enemies)
        {
            if (!aliveUnits.Any(u => u.Team == Team.Player))
            {
                yield break;
            }
            yield return new WaitForSeconds(1f);
            if (enemy != null && aliveUnits.Contains(enemy))
                enemy.StartTurn(CurrentPhase);
        }
    }

    public void StartPlayerCounter()
    {
        var player = aliveUnits.First(u => u.Team == Team.Player);
        player.StartTurn(CurrentPhase);
    }

    IEnumerator StartEnemyAction()
    {
        var enemies = aliveUnits.Where(u => u.Team == Team.Enemy).ToList();

        foreach (var enemy in enemies)
        {
            if(!aliveUnits.Any(u => u.Team == Team.Player))
            {
                CheckEndCondition();
                yield break;
            }            
            yield return new WaitForSeconds(1.5f);
            if (enemy != null && aliveUnits.Contains(enemy))
                enemy.StartTurn(CurrentPhase);
        }

    }
    public void EndEnemyPlanningPhase()
    {
        CurrentPhase = BattlePhase.PlayerCounter;
        StartPhase();
    }
    public void EndEnemyActionPhase()
    {
        CurrentPhase = BattlePhase.PlayerAction;
        StartPhase();
    }

    void StartPlayerAction()
    {
        var player = aliveUnits.First(u => u.Team == Team.Player);
        player.StartTurn(CurrentPhase);
    }

    public void EndPlayerCounterPhase()
    {
        CurrentPhase = BattlePhase.EnemyAction;
        StartPhase();
    }

    public void EndPlayerActionPhase()
    {
        CurrentPhase = BattlePhase.EnemyPlanning;
        StartPhase();
    }

    public void OnUnitRemoved(Unit unit)
    {
        aliveUnits.Remove(unit);
        CheckEndCondition();
    }
    void CheckEndCondition()
    {
        Debug.Log("Checking Conditions");
        bool playerUnitsAlive = aliveUnits.Any(u => u.Team == Team.Player);
        bool enemyUnitsAlive = aliveUnits.Any(u => u.Team == Team.Enemy);

        if (!playerUnitsAlive)
        {
            CurrentPhase = BattlePhase.Lost;
            battleEnded = true;
        }
        else if (!enemyUnitsAlive)
        {
            CurrentPhase = BattlePhase.Won;
            battleEnded = true;
        }
        if (battleEnded)
        {
            if (BattleSystemUI != null)
            {
                BattleSystemUI.UpdateText(CurrentPhase);
            }
            Debug.Log("Battle Ended");
            BattleManager.Instance.EndBattle();
        }
    }
}
