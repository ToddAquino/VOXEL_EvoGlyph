using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public enum BattleZone
{
    Fire,
    Lightning,
    Water,
    Default
}
public class BattleManager : MonoBehaviour
{
    public UnityEvent OnGameOver;
    public event Action<BattlePhase> OnBattleEnd;
    
    public static BattleManager Instance;
    public BattleController Controller;
    [Header("Zone Settings")]
    public BattleZone BattleZoneType;
    public SpriteRenderer Background;
    public Sprite FireBackground;
    public Sprite LightningBackground;
    public Sprite WaterBackground;
    public Sprite DefaultBackground;

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

    [Header("TUtorial Settings")]
    public bool isInTutorial = false;
    public bool canPickBasicAttack = true;
    public bool canPickCast = true;
    public bool canPickDefend = true;

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
        GameManager.Instance.SetState(GameState.Battle);
        SetBackground();
        StartBattle();
    }

    public void SetBackground()
    {
        ElementType element = GameManager.Instance.ExplorationData.CurrentAreaType;
        switch (element)
        {
            case ElementType.Fire:
                Background.sprite = FireBackground;
                break;

            case ElementType.Lightning:
                Background.sprite = LightningBackground;
                break;

            case ElementType.Water:
                Background.sprite = WaterBackground;
                break;

            default:
                Background.sprite = default; 
                break;
        }
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

    public void SetOnlyBasicAttack()
    {
        canPickBasicAttack = true;
        canPickCast = false;
        canPickDefend = false;
        ApplyButtonState();
    }

    public void SetOnlyCast()
    {
        canPickBasicAttack = false;
        canPickCast = true;
        canPickDefend = false;

        ApplyButtonState();
    }

    public void SetOnlyDefend()
    {
        canPickBasicAttack = false;
        canPickCast = false;
        canPickDefend = true;

        ApplyButtonState();
    }

    public void SetOnlyAttackAndDefend()
    {
        canPickBasicAttack = true;
        canPickCast = false;
        canPickDefend = true;

        ApplyButtonState();
    }

    void ApplyButtonState()
    {
        ClearAllButtonListeners();

        PlayerController playerController = playerUnit.Controller as PlayerController;
        if (playerController == null) return;

        // Cast
        var cast = ActionButtons[0].GetComponent<UnityEngine.UI.Button>();
        //cast.gameObject.SetActive(canPickCast);
        if (canPickCast)
            cast.onClick.AddListener(playerController.ActionPickedCast);

        // Attack
        var basicAttack = ActionButtons[1].GetComponent<UnityEngine.UI.Button>();
        //basicAttack.gameObject.SetActive(canPickBasicAttack);
        if (canPickBasicAttack)
            basicAttack.onClick.AddListener(playerController.ActionPickedBasicAttack);

        // Defend
        var defend = ActionButtons[2].GetComponent<UnityEngine.UI.Button>();
        //defend.gameObject.SetActive(canPickDefend);
        if (canPickDefend)
            defend.onClick.AddListener(playerController.ActionPickedDefend);
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
            HandleTomePieceDrop();
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

    public void HandleTomePieceDrop()
    {
        ExplorationData data = GameManager.Instance.ExplorationData;
        ElementType areaElement = data.CurrentAreaType;
        ElementType enemyElement = data.CurrentEncounteredEnemy.GetEnemyData().Element.Type;

        if(areaElement == enemyElement)
        {
            GameManager.Instance.PlayerData.AddTomePiece(areaElement, 1);
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
    public void SetInTutorialFalse()
    {
        isInTutorial = false;
        playerUnit.GetComponent<PlayerController>().isInTutorial = this.isInTutorial;
        enemyUnit.GetComponent<AIController>().isInTutorial = this.isInTutorial;
    }
    void SpawnPlayer()
    {
        GameObject obj = Instantiate(playerUnitPrefab, playerSpawn.position, Quaternion.identity);

        playerUnit = obj.GetComponent<PlayerUnit>();

        playerUnit.playerData = GameManager.Instance.PlayerData;
        playerUnit.SpawnPosition = playerSpawn.position;
        playerUnit.Initialize();

        Controller.aliveUnits.Add(playerUnit);

        SetupActionButtons(obj.GetComponent<PlayerController>());
    }

    void SetupActionButtons(PlayerController controller)
    {
        if(canPickCast)
        {
            UnityEngine.UI.Button cast = ActionButtons[0].GetComponent<UnityEngine.UI.Button>();
            cast.onClick.RemoveAllListeners();
            cast.onClick.AddListener(controller.ActionPickedCast);

        }
        if (canPickBasicAttack)
        {
            UnityEngine.UI.Button basicAttack = ActionButtons[1].GetComponent<UnityEngine.UI.Button>();
            basicAttack.onClick.RemoveAllListeners();
            basicAttack.onClick.AddListener(controller.ActionPickedBasicAttack);

        }
        if(canPickDefend)
        {
            UnityEngine.UI.Button defend = ActionButtons[2].GetComponent<UnityEngine.UI.Button>();
            defend.onClick.RemoveAllListeners();
            defend.onClick.AddListener(controller.ActionPickedDefend);

        }
    }
    void ClearAllButtonListeners()
    {
        foreach (var btnObj in ActionButtons)
        {
            var btn = btnObj.GetComponent<UnityEngine.UI.Button>();
            btn.onClick.RemoveAllListeners();
        }
    }

    void SpawnEnemy()
    {
        EnemyUnitData enemyData = GameManager.Instance.ExplorationData.CurrentEncounteredEnemy.GetEnemyData();
        enemyUnitPrefab = enemyData.EnemyUnitPrefab;
        GameObject prefab = enemyUnitPrefab;
        GameObject obj = Instantiate(prefab, enemySpawn.position, Quaternion.identity);

        EnemyUnit enemy = obj.GetComponent<EnemyUnit>();
        enemy.enemyUnitData = enemyData;

        enemy.SpawnPosition = enemySpawn.position;
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
        string scene = GameManager.Instance.ExplorationData.currentExplorationScene;
        GameSceneManager.Instance.LoadScene(scene);
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
