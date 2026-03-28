using System.Collections.Generic;
using UnityEngine;

public enum ExploreState
{ 
    Idle,
    InBattle,
    Won,
    Lost
}
public class ExplorationData : MonoBehaviour
{
    public Vector3 LastPlayerPosition;
    public Vector3 LastSpawnPointPosition;
    public ExploreState State = ExploreState.Idle;
    public EncounteredEnemy CurrentEncounteredEnemy = new EncounteredEnemy();
    [Header("Enemy")]
    public List<string> DefeatedEnemies = new List<string>();
    public List<string> ExistingEnemies = new List<string>();
    [Header("Tome")]
    public List<string> TomeLooted = new List<string>();
    public List<string> TomePieceCollected = new List<string>();
    public List<string> GateUnlocked = new List<string>();
    public List<string> CutscenesFinished = new List<string>();
    public string currentExplorationScene;
    public int currentAreaIndex;
    public Vector3 GetPlayerPosition()
    {
        switch (State)
        {
            case ExploreState.Won:
                return LastPlayerPosition;

            case ExploreState.Lost:
                return LastSpawnPointPosition;

            default:
                return LastSpawnPointPosition;
        }
    }
    public void RegisterExistingEnemy(string enemyID)
    {
        if (!ExistingEnemies.Contains(enemyID))
            ExistingEnemies.Add(enemyID);
    }
    public void RegisterDefeatedEnemy(string enemyID)
    {
        if (!DefeatedEnemies.Contains(enemyID))
            DefeatedEnemies.Add(enemyID);
    }
    public void RegisterLootedTome(string tomeID)
    {
        if (!TomeLooted.Contains(tomeID))
            TomeLooted.Add(tomeID);
    }
    public void RegisterCollectedTomePiece(string pieceID)
    {
        if (!TomePieceCollected.Contains(pieceID))
            TomePieceCollected.Add(pieceID);
    }
    public bool IsEnemyExists(string enemyID)
    {
        return ExistingEnemies.Contains(enemyID);
    }
    public bool IsEnemyDefeated(string enemyID)
    {
        return DefeatedEnemies.Contains(enemyID);
    }

    public bool IsTomeLooted(string tomeID)
    {
        return TomeLooted.Contains(tomeID);
    }
    public bool isTomePieceCollected(string tomeID)
    {
        return TomePieceCollected.Contains(tomeID);
    }

    public void RegisterUnlockedGate(string gateID)
    {
        if (!GateUnlocked.Contains(gateID))
            GateUnlocked.Add(gateID);
    }

    public bool IsGateUnlocked(string gateID)
    {
        return GateUnlocked.Contains(gateID);
    }
    public void RegisterCutsceneFinished(string gateID)
    {
        if (!CutscenesFinished.Contains(gateID))
            CutscenesFinished.Add(gateID);
    }

    public bool IsCutsceneFinished(string gateID)
    {
        return CutscenesFinished.Contains(gateID);
    }
}

[System.Serializable]
public class EncounteredEnemy
{
    string EnemyID;
    EnemyUnitData EnemyData;
    public string GetEnemyID()
    {
        return EnemyID;
    }
    public EnemyUnitData GetEnemyData()
    {
        return EnemyData;
    }
    public void SetEncounteredEnemy(EnemyEncounter enemy)
    {
        EnemyData = enemy.GetEnemyData();
        EnemyID = enemy.GetEnemyID();
    }
}