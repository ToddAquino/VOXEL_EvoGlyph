using UnityEngine;

public class ChangeSceneButton : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        GameSceneManager.Instance.LoadScene(sceneName);
    }
}
