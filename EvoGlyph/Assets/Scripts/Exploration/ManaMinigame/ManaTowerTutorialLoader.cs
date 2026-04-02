using UnityEngine;

public class ManaTowerTutorialLoader : MonoBehaviour,IInteractable
{
    [SerializeField] string SceneToLoad;
    public void Interact(MovingPlayerController player)
    {
        GameSceneManager.Instance.LoadScene(SceneToLoad);
    }
}
