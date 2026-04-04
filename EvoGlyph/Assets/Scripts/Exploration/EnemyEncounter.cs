using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyEncounter : MonoBehaviour
{
    [SerializeField] string enemyID;
    [SerializeField] string SceneToLoad = "BattleRoom";
    [SerializeField] EnemyUnitData enemyData;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Encounters[] possibleEnemyTransformations;
    [SerializeField] bool isEnemyDataRandom;
    [SerializeField] AIMovementComponent MovementComponent;

    bool isAlive = true;

    public void Instantiate()
    {
        ExplorationData data = GameManager.Instance.ExplorationData;
        if (data.IsEnemyDefeated(GetEnemyID()))
        {
            this.gameObject.SetActive(false);
            isAlive = false;
            return;
        }
        if (MovementComponent != null)
        {
            MovementComponent.Initialize();
        }
        gameObject.SetActive(true);
        isAlive = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isAlive)
        {
            Interact();
        }
    }

    public void Interact()
    {
        isAlive = false;
        Debug.Log("Encountered Enemy");
        if (isEnemyDataRandom)
        {
            SetRandomEnemy();
        }
        GameManager.Instance.ExplorationData.LastPlayerPosition = this.transform.position;
        GameManager.Instance.ExplorationData.CurrentEncounteredEnemy.SetEncounteredEnemy(this);
        GameSceneManager.Instance.LoadScene(SceneToLoad);
    }

    public void SetRandomEnemy()
    {
        //int randInt = Random.Range(0,possibleEnemyTransformations.Length);
        //enemyData = possibleEnemyTransformations[randInt];
        float totalChance = 0f;

        foreach (var encounter in possibleEnemyTransformations)
        {
            totalChance += encounter.TransformationChance;
        }

        float randInt = Random.Range(0, totalChance);
        float current = 0f;

        foreach (var encounter in possibleEnemyTransformations)
        {
            current += encounter.TransformationChance;

            if (randInt <= current)
            {
                enemyData = encounter.PossibleEnemyTransformation;
                return;
            }
        }
    }

    public string GetEnemyID()
    {
        return enemyID;
    }

    public EnemyUnitData GetEnemyData()
    {
        return enemyData;
    }
}
[System.Serializable]
public class Encounters
{
    public EnemyUnitData PossibleEnemyTransformation; //Type of enemy
    public float TransformationChance; //chance of type enemy to appear
}