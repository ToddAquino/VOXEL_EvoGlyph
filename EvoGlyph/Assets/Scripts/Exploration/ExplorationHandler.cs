using System.Collections.Generic;
using UnityEngine;
public class ExplorationHandler : MonoBehaviour
{
    public static ExplorationHandler Instance;
    [SerializeField] List<Area> areaList;

    [Header("Current Environment Data")]
    [SerializeField] Area currentArea;
    [SerializeField] GameObject Player;
    
    void Awake()
    {
        Instance = this;
            
    }

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        currentArea = areaList[GameManager.Instance.ExplorationData.currentAreaIndex];
        ExplorationData data = GameManager.Instance.ExplorationData;

        //SetCurrentAreaSpawnPoint(currentArea);

        //foreach (Checkpoint checkpoint in currentArea.Checkpoints)
        //{
        //    checkpoint.CheckpointSet += SetCurrentCheckpoint;
        //}
        foreach (EnemyEncounter encounter in currentArea.EnemyEncounters)
        {
            // Only spawn if not defeated
            if (data.IsEnemyDefeated(encounter.GetEnemyID()))
            {
                encounter.gameObject.SetActive(false);
            }
        }
        foreach (TomePickup pickup in currentArea.TomePickups)
        {
            if (data.IsTomeLooted(pickup.GetTomeID()))
            {
                pickup.gameObject.SetActive(false);
            }
        }
        foreach (Gate gate in currentArea.Gates)
        {
            gate.Initialize(data.IsGateUnlocked(gate.GetGateID()));
            gate.gateKey.OnUnlock += gate.UnlockGate;
        }
        foreach (TimelineController cutscenes in currentArea.Cutscenes)
        {
            if (data.IsCutsceneFinished(cutscenes.GetCutsceneID()))
            {
                cutscenes.gameObject.SetActive(false);
            }
        }
        //Player.transform.position = GameManager.Instance.ExplorationData.GetPlayerPosition();
        Player.transform.position = GameManager.Instance.ExplorationData.GetPlayerPosition();
        //audio here
        switch (GameManager.Instance.ExplorationData.currentAreaIndex)
        {
            case 0:
                AudioManager.Instance.PlayMusic("exploration");
                break;

            case 1:
                AudioManager.Instance.PlayMusic("area1E");
                break;

            case 2:
                AudioManager.Instance.PlayMusic("exploration");
                break;

            default:
                Debug.LogWarning($"No music assigned for area index {GameManager.Instance.ExplorationData.currentAreaIndex}");
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //void SetCurrentAreaSpawnPoint(Area area)
    //{
    //    GameManager.Instance.ExplorationData.LastSpawnPointPosition = area.SpawnPoint.transform.position;
    //}

    //void SetCurrentCheckpoint(Checkpoint checkpoint)
    //{
    //    GameManager.Instance.ExplorationData.LastCheckpointPosition = checkpoint.transform.position;
    //}

    //public void SetLastPosition(Vector3 transform)
    //{
    //    GameManager.Instance.ExplorationData.LastPlayerPosition = transform;
    //}
}

[System.Serializable]
public class Area
{
    public Checkpoint SpawnPoint;
    public List<Checkpoint> Checkpoints;
    public List<EnemyEncounter> EnemyEncounters;
    public List<TomePickup> TomePickups;
    public List<Gate> Gates;
    public List<TimelineController> Cutscenes;
}