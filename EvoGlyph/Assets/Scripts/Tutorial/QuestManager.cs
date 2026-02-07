using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    [SerializeField] Quest currentQuest;
    private int currentStepIndex;
    private QuestStep currentStep;

    private void Awake()
    {
        Instance = this;
    }
    public void SetActiveQuest(Quest targetQuest)
    {
        currentQuest = targetQuest;
    }

    public void StartQuest()
    {
        currentStepIndex = 0;
        currentQuest.StartQuestStep();
    }
}
