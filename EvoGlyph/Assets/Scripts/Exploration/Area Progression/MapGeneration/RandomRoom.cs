using System.Collections.Generic;
using UnityEngine;

public class RandomRoom : RoomController
{
    [Header("TomePiece")]
    [SerializeField] TomePiece piece;

    [Header("Enemy")]
    [SerializeField] int minEnemies = 1;
    [SerializeField] int maxEnemies;
    [SerializeField] EnemyEncounter[] enemyList;
    [SerializeField] List<EnemyEncounter> existingEnemies; 
    [SerializeField] bool isInitialized = false;
    public bool isRandomized = false;
    void Start()
    {
        
    }

    public override void Initialize()
    {
        if (isInitialized) return;

        isInitialized = true;
        if (!isRandomized)
        {
            SetRandomEnemies();
        }
        if (piece != null)
        {
            piece.gameObject.SetActive(false);
        }
        SetExistingEnemies();
        ActivateEnemies();
        CheckIfRoomCleared();
        InitializeGates();
    }

    void InitializeGates()
    {
        foreach (Gate gate in RoomGates)
        {
            gate.Initialize(isRoomCleared);
        }
    }
    void CheckIfRoomCleared()
    {
        if (existingEnemies.Count == 0 && piece == null) return;

        foreach (var enemy in existingEnemies)
        {
            if(!GameManager.Instance.ExplorationData.DefeatedEnemies.Contains(enemy.GetEnemyID()))
            {
                Debug.Log("AreaNotCleared");
                return;
            }
        }
        isRoomCleared = true;
        ShowTomePiece();
    }

    void ShowTomePiece()
    {
        if (piece != null)
        {
            piece.gameObject.SetActive(true);
        }
    }
    void SetRandomEnemies()
    {
        isRandomized = true;
        existingEnemies.Clear();
        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);
        enemyCount = Mathf.Min(enemyCount, enemyList.Length);

        List<EnemyEncounter> tempList = new List<EnemyEncounter>(enemyList);
        for (int i = 0; i < tempList.Count; i++)
        {
            int randIndex = Random.Range(i, tempList.Count);
            (tempList[i], tempList[randIndex]) = (tempList[randIndex], tempList[i]);
        }
        for (int i = 0; i < enemyCount; i++)
        {
            existingEnemies.Add(tempList[i]);
            GameManager.Instance.ExplorationData.RegisterExistingEnemy(tempList[i].GetEnemyID());
        }      
    }
    void SetExistingEnemies()
    {
        foreach(EnemyEncounter enemy in enemyList)
        {
            if(GameManager.Instance.ExplorationData.IsEnemyExists(enemy.GetEnemyID()))
            {
                existingEnemies.Add(enemy);
            }
        }
    }
    void ActivateEnemies()
    {
        foreach(EnemyEncounter enemy in enemyList)
        {
            if(existingEnemies.Contains(enemy))
            {
                enemy.Instantiate();
            }
            else
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }


}
