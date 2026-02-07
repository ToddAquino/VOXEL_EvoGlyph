using UnityEngine;
using UnityEngine.Events;
public abstract class QuestStep : MonoBehaviour
{
    public UnityEvent OnInitialized;
    public UnityEvent OnStepFinished;
    public UnityEvent OnStepFailed;
    private bool isFinished = false;
    
    public void InitializeStep()
    {
        isFinished = false;
        EnableStep();
        OnInitialized?.Invoke();
    }
    public void FinishQuestStep()
    {
        if (isFinished) return; 
        
        isFinished = true;
        DisableStep();
        OnStepFinished?.Invoke();
    }

    protected abstract void EnableStep();
    protected abstract void DisableStep();

    public void FailedQuestStep()
    {
        OnStepFailed?.Invoke();
    }
}
