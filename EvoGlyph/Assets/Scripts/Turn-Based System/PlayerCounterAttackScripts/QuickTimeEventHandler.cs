using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum QuickTimeEventResult
{
    None,
    Success,
    Perfect,
    Failed
}
public class QuickTimeEventHandler : MonoBehaviour, IPointerClickHandler
{
    public event Action<QuickTimeEventResult> OnQTEFinished;
    //[SerializeField] PlayerCounterPhaseHandler resultHandler;
    [SerializeField] private Image ProgressBar;
    [SerializeField] private Image successWindowImage;
    [SerializeField] private Image perfectWindowImage;

    public bool IsActive;
    float maxTime;
    public float TimeRemaining;

    [Header("QTE Settings")]
    [SerializeField] float successWindowStart = 0.65f;
    [SerializeField] float successWindowEnd = 1f;
    [SerializeField] float perfectWindowStart = 0.8f;
    [SerializeField] float perfectWindowEnd = 0.9f;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsActive) return;
        float progress = 1 - TimeRemaining / maxTime;

        if (progress >= perfectWindowStart && progress <= perfectWindowEnd)
        {
            AudioManager.Instance.PlaySFX("parried", 0.4f);
            EndQuickTimeEvent(QuickTimeEventResult.Perfect);
        }
            
        else if (progress >= successWindowStart && progress <= successWindowEnd)
        {
            AudioManager.Instance.PlaySFX("blocked", 0.4f);
            EndQuickTimeEvent(QuickTimeEventResult.Success);
        }
        else
        { 
            EndQuickTimeEvent(QuickTimeEventResult.Failed);
        }
        
    }

    public void StartQuickTimeEvent(float duration)
    {
        maxTime = duration;
        TimeRemaining = maxTime;
        IsActive = true;
        SetupWindow(successWindowImage, successWindowStart, successWindowEnd);
        SetupWindow(perfectWindowImage, perfectWindowStart, perfectWindowEnd);
        ProgressBar.fillAmount = 1;
    }
    void SetupWindow(Image image, float start, float end)
    {
        float windowSize = end - start;

        image.fillAmount = windowSize;

        float startAngle = start * 360f;

        image.rectTransform.localRotation = Quaternion.Euler(0, 0, startAngle);
    }
    void EndQuickTimeEvent(QuickTimeEventResult result)
    {
        IsActive = false;
        OnQTEFinished?.Invoke(result);
    }
    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            TimeRemaining -= Time.deltaTime;
            if (ProgressBar != null)
            {
                ProgressBar.fillAmount = 1 - TimeRemaining / maxTime;
            }

            if (TimeRemaining <= 0)
            {
                EndQuickTimeEvent(QuickTimeEventResult.Failed);
            }
        }
    }
}
