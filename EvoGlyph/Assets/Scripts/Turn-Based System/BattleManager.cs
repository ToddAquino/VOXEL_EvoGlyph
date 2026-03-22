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

    [Header("Glyph System")]
    public GlyphBoard glyphBoard;
    public GlyphController controller;
    [SerializeField] GameObject[] ActionButtons;

    //[SerializeField] GlyphController glyphController;
    [Header("Unit Prefabs")]
    public GameObject playerUnitPrefab;
    public GameObject enemyUnitPrefab;
    [SerializeField] Transform playerSpawn;
    [SerializeField] Transform enemySpawn;

    [Header("Stored Units")]
    public PlayerUnit playerUnit;
    public EnemyUnit enemyUnit;
    private Coroutine loadWaveCoroutine = null;
    public bool isInfiniteBattle;
    public int currentWave = 0;
    public bool autoStartBattle = true;
    public bool isInTutorial = false;

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
        //if (!autoStartBattle) return;
        //GlyphBoard.Instance.GenerateField();
        StartBattle();
    }
    public void ShowActionOptions()
    {
        foreach (var btn in ActionButtons)
        {
            btn.SetActive(true);
        }
    }
    public void HideActionOptions()
    {
        foreach (var btn in ActionButtons)
        {
            btn.SetActive(false);
        }
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
        else if (!isInfiniteBattle && Controller.CurrentPhase == BattlePhase.Won)
        {
            //Track defeate enemy
            GameManager.Instance.ExplorationData.RegisterDefeatedEnemy(
            GameManager.Instance.ExplorationData.CurrentEncounteredEnemy.GetEnemyID());
            DoManaGain();
            GameManager.Instance.ExplorationData.State = ExploreState.Won;
            OnGameOver?.Invoke();
        }
        else if (Controller.CurrentPhase == BattlePhase.Lost)
        {
            GameManager.Instance.ExplorationData.State = ExploreState.Lost;
            OnGameOver?.Invoke();
        }
        Debug.Log(Controller.CurrentPhase.ToString());
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

        SpawnPlayer();
        SpawnEnemy();
        playerUnit.GetComponent<PlayerController>().isInTutorial = this.isInTutorial;
        enemyUnit.GetComponent<AIController>().isInTutorial = this.isInTutorial;
        playerUnit.SetTarget(enemyUnit);
        enemyUnit.SetTarget(playerUnit);
        //playerUnit.Initialize();
        //Controller.aliveUnits.Add(playerUnit);
        //foreach (Unit unit in enemyUnits)
        //{
        //    unit.Initialize();
        //    Controller.aliveUnits.Add(unit);
        //}
    }

    void SpawnPlayer()
    {
        GameObject obj = Instantiate(playerUnitPrefab, playerSpawn.position, Quaternion.identity);

        playerUnit = obj.GetComponent<PlayerUnit>();

        playerUnit.playerData = GameManager.Instance.PlayerData;

        playerUnit.Initialize();

        Controller.aliveUnits.Add(playerUnit);

        SetupActionButtons(obj.GetComponent<PlayerController>());
    }

    void SetupActionButtons(PlayerController controller)
    {
        UnityEngine.UI.Button cast = ActionButtons[0].GetComponent<UnityEngine.UI.Button>();
        UnityEngine.UI.Button basicAttack = ActionButtons[1].GetComponent<UnityEngine.UI.Button>();
        UnityEngine.UI.Button defend = ActionButtons[2].GetComponent<UnityEngine.UI.Button>();

        basicAttack.onClick.RemoveAllListeners();
        cast.onClick.RemoveAllListeners();
        defend.onClick.RemoveAllListeners();

        basicAttack.onClick.AddListener(controller.ActionPickedBasicAttack);
        cast.onClick.AddListener(controller.ActionPickedCast);
        defend.onClick.AddListener(controller.ActionPickedDefend);
    }

    void SpawnEnemy()
    {
        EnemyUnitData enemyData = GameManager.Instance.ExplorationData.CurrentEncounteredEnemy.GetEnemyData();
        enemyUnitPrefab = enemyData.EnemyUnitPrefab;
        GameObject prefab = enemyUnitPrefab;
        GameObject obj = Instantiate(prefab, enemySpawn.position, Quaternion.identity);

        EnemyUnit enemy = obj.GetComponent<EnemyUnit>();
        enemy.enemyUnitData = GameManager.Instance.ExplorationData.CurrentEncounteredEnemy.GetEnemyData();

        enemy.Initialize();

        enemyUnit = enemy;

        Controller.aliveUnits.Add(enemy);
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
        playerUnit = null;
        enemyUnit = null;
        foreach (Unit unit in Controller.aliveUnits)
        {
            if (unit == null) continue;

            unit.Deinitialize();
            //Destroy(unit.gameObject);
        }
        Controller.aliveUnits.Clear();
    }

    public void OnUnitDied(Unit unit)
    {        
        Controller.OnUnitRemoved(unit);
    }

    void DoManaGain()
    {
        EnemyUnitData data = GameManager.Instance.ExplorationData.CurrentEncounteredEnemy.GetEnemyData();
        if (data.RollManaChance())
        {
            PlayerData playerData = GameManager.Instance.PlayerData;
            if (playerData != null)
            {
                playerData.RefillMana(data.ManaDropAmount);
                Debug.Log($"Gain: {data.ManaDropAmount} Have: {playerData.CurrentMana}");
            }
        }
    }

    public void ReturnToMenu()
    {
        GameSceneManager.Instance.LoadScene("Exploration");
    }

    public void Retry()
    {
        GameSceneManager.Instance.LoadScene("InfiniteBattleRoom");
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
