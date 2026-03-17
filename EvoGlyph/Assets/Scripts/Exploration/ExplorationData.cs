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
    public Vector3 LastCheckpointPosition;
    public Vector3 LastPlayerPosition;
    public ExploreState State = ExploreState.Idle;
    public EncounteredEnemy CurrentEncounteredEnemy = new EncounteredEnemy();
    public List<string> DefeatedEnemies = new List<string>();
    public Vector3 GetPlayerPosition()
    {
        switch (State)
        {
            case ExploreState.Won:
                return LastPlayerPosition;

            default:
                return LastCheckpointPosition;

        }
    }
    public void RegisterDefeatedEnemy(EnemyUnitData data)
    {
        if (!DefeatedEnemies.Contains(data.EnemyID))
            DefeatedEnemies.Add(data.EnemyID);
    }
    public bool IsEnemyDefeated(string enemyID)
    {
        return DefeatedEnemies.Contains(enemyID);
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