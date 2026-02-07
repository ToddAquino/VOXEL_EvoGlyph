using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    public UnityEvent OnTutorialComplete;
    public UnityEvent SetupTutorial;
    UnityAction dialogueEndAction;
    [Header("Steps")]
    public TutorialStep[] TutorialSteps;
    [SerializeField] private int currentTutorialStepIndex;
    [SerializeField] private float nextStepDelay;
    [SerializeField] TutorialMenuNodeData tutorialID;

    void Start()
    {
        SetupTutorial?.Invoke();
        currentTutorialStepIndex = 0;
        TutorialManager.Instance.SetActiveQuest(this);
        TutorialManager.Instance.StartQuest();
    }

    // Update is called once per frame

    public void RestartTutorial()
    {
        StopAllCoroutines();

        DialogueManager.Instance.OnConversationEnd.RemoveAllListeners();
        foreach (var step in TutorialSteps)
        {
            if (step.questStep != null)
                step.questStep.OnStepFinished.RemoveAllListeners();
        }
        currentTutorialStepIndex = 0;
        TutorialManager.Instance.SetActiveQuest(this);
        TutorialManager.Instance.StartQuest();
    }
    public void StartTutorialStep()
    {
        if (currentTutorialStepIndex >= TutorialSteps.Length) return;

        TutorialStep step = TutorialSteps[currentTutorialStepIndex];
        // Play dialogue first (if exists)
        if (step.dialogueIntro?.DialgoueText != null && step.dialogueIntro?.DialgoueText.Count > 0)
        {
            dialogueEndAction = () => OnDialogueFinished(step);
            DialogueManager.Instance.OnConversationEnd.AddListener(dialogueEndAction);

            DialogueManager.Instance.ActivateDialogue(step.dialogueIntro.DialgoueText);
        }
        else
        {
            StartGameplayStep(step);
        }

    }

    private void OnDialogueFinished(TutorialStep step)
    {
        DialogueManager.Instance.OnConversationEnd.RemoveListener(dialogueEndAction);
        step.dialogueIntro.OnLineEnd?.Invoke();
        if (step.hideDialogueOnEnd)
            DialogueManager.Instance.HideDialogueBox();

        StartGameplayStep(step);
    }
    private void StartGameplayStep(TutorialStep step)
    {
        Debug.Log("Starting SkillCheck");
        if (step.questStep == null)
        {
            MoveToNextStep();
            return;
        }

        step.questStep.InitializeStep();
        step.questStep.OnStepFinished.AddListener(OnStepCompleted);
    }

    private void MoveToNextStep()
    {
        DialogueManager.Instance.OnConversationEnd.RemoveListener(MoveToNextStep);
        TutorialStep step = TutorialSteps[currentTutorialStepIndex];

        if (step.hideDialogueOnEnd)
            DialogueManager.Instance.HideDialogueBox();

        currentTutorialStepIndex++;

        if (currentTutorialStepIndex > TutorialSteps.Length - 1)
        {
            OnQuestComplete();
        }
        else
        {
            StartCoroutine(DelayNextTutorialStep());
        }
    }

    private IEnumerator DelayNextTutorialStep()
    {
        yield return new WaitForSeconds(nextStepDelay);
        StartTutorialStep();
    }
    private void OnStepCompleted()
    {
        Debug.Log("Completed SkillCheck");
        if (currentTutorialStepIndex >= TutorialSteps.Length) return;
        TutorialSteps[currentTutorialStepIndex].questStep.OnStepFinished.RemoveListener(OnStepCompleted);


        TutorialStep step = TutorialSteps[currentTutorialStepIndex];
        // Play dialogue first (if exists)
        if (step.dialogueFeedback?.DialgoueText != null && step.dialogueFeedback?.DialgoueText.Count > 0)
        {
            if (step.hideDialogueOnEnd)
            {
                DialogueManager.Instance.gameObject.SetActive(true);
            }
            DialogueManager.Instance.ClearDialogue();
            DialogueManager.Instance.OnConversationEnd.AddListener(MoveToNextStep);

            DialogueManager.Instance.ActivateDialogue(step.dialogueFeedback.DialgoueText);
        }
        else
        {
            MoveToNextStep();
        }
   
    }
    private void OnQuestComplete()
    {
        Debug.Log("Tutorial Quest Completed");
        OnTutorialComplete?.Invoke();
    }
}

