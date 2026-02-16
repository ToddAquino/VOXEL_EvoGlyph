using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum QuickTimeEventResult
{
    None,
    Success,
    Failed
}
public class QuickTimeEventHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] PlayerCounterPhaseHandler resultHandler;
    [SerializeField] private Image ProgressBar;
    event Action OnClick;
    public QuickTimeEventResult Result;
    public bool IsQuickTimeEventActive;
    float maxTime;
    public float TimeProgress;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsQuickTimeEventActive) return;
        OnClick?.Invoke();
    }

    public void StartQuickTimeEvent(float duration)
    {
        ProgressBar.fillAmount = 1;
        maxTime = duration;
        Result = QuickTimeEventResult.None;
        this.OnClick += OnQuickTimeEventActionMet;
        TimeProgress = maxTime;
        IsQuickTimeEventActive = true;
    }

    void EndQuickTimeEvent()
    {
        this.OnClick -= OnQuickTimeEventActionMet;
        IsQuickTimeEventActive = false;
        resultHandler.OnQTEFinish(Result);
    }
    // Update is called once per frame
    void Update()
    {
        if (IsQuickTimeEventActive)
        {
            if (TimeProgress > 0)
            {
                TimeProgress -= Time.deltaTime;
                if (ProgressBar != null)
                {
                    ProgressBar.fillAmount = TimeProgress / maxTime;
                }
            }
            if (TimeProgress <= 0 && Result == QuickTimeEventResult.None)
            {
                OnQuickTimeEventActionMissed();
            }
        }
    }

    void OnQuickTimeEventActionMet()
    {
        Result = QuickTimeEventResult.Success;
        EndQuickTimeEvent();
    }

    void OnQuickTimeEventActionMissed()
    {
        Result = QuickTimeEventResult.Failed;
        EndQuickTimeEvent();
    }
}
