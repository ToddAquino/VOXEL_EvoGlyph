using UnityEngine;
using UnityEngine.SceneManagement;

public class TimelineSignalReciever : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        GameSceneManager.Instance.LoadScene(sceneName);
    }
}
