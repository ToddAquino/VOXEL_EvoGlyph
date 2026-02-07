using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    [SerializeField] Tutorial currentTutorial;
    //private int currentStepIndex;
    private TutorialStep currentStep;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetActiveQuest(Tutorial obj)
    {
        currentTutorial = obj;
    }

    public void StartQuest()
    {
        //currentStepIndex = 0;
        currentTutorial.StartTutorialStep();
    }
}
