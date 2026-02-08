using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class BattleManager : MonoBehaviour
{
    public event Action<BattleState> OnBattleEnd;
    public static BattleManager Instance;
    public BattleController Controller;
    [SerializeField] GlyphController glyphController;
    [Header("Unit Prefabs")]
    public Unit playerUnit;
    public Unit[] enemyUnits;

    public bool isInfiniteBattle;
    public int currentWave = 0;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GlyphBoard.Instance.GenerateField();
        StartBattle();
    }

    public void StartBattle()
    {
        InitializeUnits();
        ArrangeOrderByUnitSpeed();
        Controller.Initialize();
        Controller.ChangeNextActiveUnit();
    }
    public void EndBattle()
    {
        OnBattleEnd?.Invoke(Controller.state);
        if (isInfiniteBattle && Controller.state == BattleState.Won)
        {
            StartCoroutine(LoadNewWave());
            //return;
        }

    }
    IEnumerator LoadNewWave()
    {
        yield return new WaitUntil(() => !glyphController.isTimerActive);
        //yield return new WaitForSeconds(1f); //bruh attach to timer
        StartNextWave();
    }

    void InitializeUnits()
    {
        DeinitializeUnits();
        Controller.aliveUnits.Clear();
        playerUnit.Initialize();
        Controller.aliveUnits.Add(playerUnit);
        foreach (Unit unit in enemyUnits)
        {
            unit.Initialize();
            Controller.aliveUnits.Add(unit);
        }

    }

    void ArrangeOrderByUnitSpeed()
    {
        Controller.aliveUnits.Sort((a, b) => b.Speed.CompareTo(a.Speed));
    }
    void StartNextWave()
    {
        currentWave++;
        Debug.Log($"Starting Wave {currentWave}");

        InitializeUnits();
        ArrangeOrderByUnitSpeed();
        Controller.Initialize();
        Controller.ChangeNextActiveUnit();
    }

    void DeinitializeUnits()
    {
        foreach (Unit unit in Controller.aliveUnits)
        {
            unit.Deinitialize();
        }
        Controller.aliveUnits.Clear();
    }

    public void OnUnitDied(Unit unit)
    {        
        Controller.OnUnitRemoved(unit);
    }
}
