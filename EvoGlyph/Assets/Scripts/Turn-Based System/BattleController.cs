using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BattlePhase
{
    PlayerAction,
    EnemyAction,
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
        if (BattleManager.Instance.autoStartBattle)
        {
            CurrentPhase = BattlePhase.PlayerAction;
            StartPhase();
        }
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

            case BattlePhase.EnemyAction:
                StartCoroutine(StartEnemyAction());
                break;

            case BattlePhase.PlayerAction:
                StartCoroutine(StartPlayerAction());
                break;
        }
    }

    IEnumerator StartEnemyAction()
    {
        var enemies = aliveUnits.Where(u => u.Team == Team.Enemy).ToList();

        foreach (var enemy in enemies)
        {

            if (!aliveUnits.Any(u => u.Team == Team.Player))
            {
                CheckEndCondition();
                yield break;
            }

            if (enemy != null && aliveUnits.Contains(enemy))
            {
                yield return new WaitForSeconds(1.5f);
                enemy.StartTurn(CurrentPhase);
            }
        }
    }

    public void EndEnemyActionPhase()
    {
        if (battleEnded) return;
        CurrentPhase = BattlePhase.PlayerAction;
        StartPhase();
    }

    IEnumerator StartPlayerAction()
    {
        var player = aliveUnits.First(u => u.Team == Team.Player);
        yield return new WaitForSeconds(1.5f);
        player.StartTurn(CurrentPhase);
    }

    public void EndPlayerActionPhase()
    {
        if (battleEnded) return;
        CurrentPhase = BattlePhase.EnemyAction;
        StartPhase();
    }

    public void OnUnitRemoved(Unit unit)
    {
        aliveUnits.Remove(unit);
        CheckEndCondition();
    }
    void CheckEndCondition()
    {
        if (battleEnded) return;
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
