using UnityEngine;

public class MusicZone : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] private string musicKey;
    [SerializeField] private bool stopMusicInstead = false;
    [SerializeField] private float fadeDuration = 2f;

    [Header("Zone Settings")]
    [SerializeField] private bool playOnce = false;
    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playOnce && hasPlayed)
                return;

            ChangeMusicForZone();
            hasPlayed = true;
        }
    }

    private void ChangeMusicForZone()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("AudioManager not found!");
            return;
        }

        if (stopMusicInstead)
        {
            Debug.Log($"Stopping music in zone: {gameObject.name}");
            AudioManager.Instance.StopMusic(fadeDuration);
        }
        else if (!string.IsNullOrEmpty(musicKey))
        {
            Debug.Log($"Changing music to '{musicKey}' in zone: {gameObject.name}");
            AudioManager.Instance.PlayMusic(musicKey, true, fadeDuration);
        }
    }

    // Visualize the zone in editor
    private void OnDrawGizmos()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = stopMusicInstead ? new Color(1f, 0f, 0f, 0.3f) : new Color(0f, 1f, 0f, 0.3f);

            if (col is BoxCollider2D box)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(box.offset, box.size);
            }
            else if (col is CircleCollider2D circle)
            {
                Gizmos.DrawSphere(transform.position + (Vector3)circle.offset, circle.radius);
            }
        }
    }
}
