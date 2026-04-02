using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    [SerializeField] MovingPlayerController player;
    [SerializeField] Transform initialSpawnPoint;
    [SerializeField] Vector3 currentSpawnPoint;
    //void Start()
    //{
    //    MoveToInitialSpawn();
    //}

    public void MoveToInitialSpawn()
    {
        GameManager.Instance.ExplorationData.LastSpawnPointPosition = initialSpawnPoint.position;
        player.transform.position = initialSpawnPoint.position;
    }
    public void MoveToSpawnPoint(Vector3 spawnPoint)
    {
        currentSpawnPoint = spawnPoint;
        player.transform.position = currentSpawnPoint;
    }

    public void SetLastSpawnPosition(Transform spawnPoint)
    {
        GameManager.Instance.ExplorationData.LastSpawnPointPosition = spawnPoint.position;
    }
}
