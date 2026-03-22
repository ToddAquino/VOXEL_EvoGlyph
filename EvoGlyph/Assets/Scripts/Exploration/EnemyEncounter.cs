using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyEncounter : MonoBehaviour
{
    [SerializeField] string SceneToLoad = "BattleRoom";
    [SerializeField] EnemyUnitData enemyData;
    bool isAlive = true;

    public void Instantiate()
    {
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
        ExplorationHandler.Instance.SetLastPosition(this.transform.position);
        GameManager.Instance.ExplorationData.CurrentEncounteredEnemy.SetEncounteredEnemy(this);
        GameSceneManager.Instance.LoadScene(SceneToLoad);
    }
    public string GetEnemyID()
    {
        return this.gameObject.name;
    }

    public EnemyUnitData GetEnemyData()
    {
        return enemyData;
    }
}
