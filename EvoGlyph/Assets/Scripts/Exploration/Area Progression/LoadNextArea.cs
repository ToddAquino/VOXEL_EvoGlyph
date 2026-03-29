using UnityEngine;

public class LoadNextArea : MonoBehaviour
{
    public string SceneToLoad;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            string scene = SceneToLoad;
            GameSceneManager.Instance.LoadScene(scene);
        }
    }
}
