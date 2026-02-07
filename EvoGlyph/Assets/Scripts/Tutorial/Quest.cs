using UnityEngine;

public class Quest : MonoBehaviour
{
    //public QuestSO info;
    [Header("Steps")]
    public QuestStep[] QuestSteps;
    private int currentQuestStepIndex;
   
    //public Quest(QuestSO info)
    //{
    //    this.info = info;
    //    //this.state = QuestState.RequirementsNotMet;
    //    this.currentQuestStepIndex = 0;
    //}

    public void Start()
    {
        currentQuestStepIndex = 0;
        //QuestManager.Instance.SetActiveQuest(this);
    }

    public void StartQuestStep()
    {
        if (currentQuestStepIndex >= QuestSteps.Length) return;

        QuestSteps[currentQuestStepIndex].OnStepFinished.AddListener(OnStepCompleted);
    }
    public void MoveToNextStep()
    {
        currentQuestStepIndex++;

        if (currentQuestStepIndex >= QuestSteps.Length)
        {
            OnQuestComplete();
        }
    }

    public void OnStepCompleted()
    {
        QuestSteps[currentQuestStepIndex].OnStepFinished.RemoveListener(OnStepCompleted);
        MoveToNextStep();
    }

    public void OnQuestComplete()
    {
        Debug.Log("Quest Completed");
    }
}