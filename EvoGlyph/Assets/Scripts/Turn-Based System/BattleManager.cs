using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
public class BattleManager : MonoBehaviour
{
    public UnityEvent OnGameOver;
    public event Action<BattlePhase> OnBattleEnd;
    public static BattleManager Instance;
    public BattleController Controller;
    //[SerializeField] GlyphController glyphController;
    [Header("Unit Prefabs")]
    public Unit playerUnit;
    public Unit[] enemyUnits;
    private Coroutine loadWaveCoroutine = null;
    public bool isInfiniteBattle;
    public int currentWave = 0;
    public bool autoStartBattle = true;

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
        if (!autoStartBattle) return;
        GlyphBoard.Instance.GenerateField();
        StartBattle();
    }

    public void StartBattle()
    {
        InitializeUnits();
        Controller.Initialize();
    }
    public void EndBattle()
    {
        OnBattleEnd?.Invoke(Controller.CurrentPhase);
        if (isInfiniteBattle && Controller.CurrentPhase == BattlePhase.Won)
        {
            if (loadWaveCoroutine == null)
            {
                loadWaveCoroutine = StartCoroutine(LoadNewWave());
            }
            else
            {
                //ITS FIXED
                //Debug.LogWarning("<color=red>do NOT DARE to run again</color>");
            }
            //return;
        }
        else if (isInfiniteBattle && Controller.CurrentPhase == BattlePhase.Lost)
        {
            OnGameOver?.Invoke();
        }
    }
    //IEnumerator LoadNewWave()
    //{
    //    if (glyphController != null && glyphController.isTimerActive)
    //    {
    //        float elapsed = 0f;

    //        while (glyphController.isTimerActive && elapsed < 10f)
    //        {
    //            elapsed += Time.deltaTime;
    //            yield return null;
    //        }

    //        if (elapsed >= 10f)
    //        {
    //            Debug.LogWarning("Wave load timeout timer taking too long");
    //        }
    //    }
    //    //yield return new WaitForSeconds(1f); //bruh attach to timer
    //    StartNextWave();
    //    loadWaveCoroutine = null;
    //}
    IEnumerator LoadNewWave()
    {
        Debug.Log("Loading New Wave");
        yield return new WaitForSeconds(1f);
        StartNextWave();
        loadWaveCoroutine = null;
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

    void StartNextWave()
    {
        currentWave++;
        Debug.Log($"Starting Wave {currentWave}");

        InitializeUnits();
        Controller.Initialize();
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

    public void ReturnToMenu()
    {
        GameSceneManager.Instance.LoadScene("TutorialMenu");
    }

    public void Retry()
    {
        GameSceneManager.Instance.LoadScene("InfiniteBattleRoom");
    }
}
