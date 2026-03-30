using UnityEngine;
using UnityEngine.Events;

public class TriggerCutscene: MonoBehaviour
{
    [SerializeField] TimelineController timelineController;
    [SerializeField] GameObject player;
    [SerializeField] Transform PlayerSpawnPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.Instance.ExplorationData.LastSpawnPointPosition = PlayerSpawnPoint.position;
            player.transform.position = PlayerSpawnPoint.position;
            timelineController.Play();
        }
    }
}
