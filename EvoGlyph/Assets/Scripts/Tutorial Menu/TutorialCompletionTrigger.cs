using UnityEngine;

public class TutorialCompletionTrigger : MonoBehaviour
{
    [SerializeField] private TutorialMenuNodeData tutorialID;

    public void ReturnToMenuOnComplete()
    {
        TutorialProgressManager.Instance.MarkCompleted(tutorialID);
        GameSceneManager.Instance.LoadScene("TutorialMenu");
    }
    public void MoveToNextTutorialOnComplete()
    {
        TutorialProgressManager.Instance.MarkCompleted(tutorialID);
        GameSceneManager.Instance.LoadScene(tutorialID.NextTutorialID.TutorialSceneToLoad);
    }
}
