using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyEncounter : MonoBehaviour
{
    [SerializeField] string enemyID;
    [SerializeField] string SceneToLoad = "BattleRoom";
    [SerializeField] EnemyUnitData enemyData;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] EnemyUnitData[] possibleEnemyTransformations;
    [SerializeField] bool isEnemyDataRandom;
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
        int randInt = Random.Range(0,possibleEnemyTransformations.Length);
        enemyData = possibleEnemyTransformations[randInt];
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
