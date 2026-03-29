using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum QuickTimeEventResult
{
    None,
    Success,
    Perfect,
    Failed
}
public class QuickTimeEventHandler : MonoBehaviour
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

    public bool isInTutorial;
    public bool PauseAtSuccess = false;
    public bool PauseAtPerfect = false;

    public void StartQuickTimeEvent(float duration, bool inTutorial)
    {
        isInTutorial = inTutorial;

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
        Time.timeScale = 1f;
        IsActive = false;
        OnQTEFinished?.Invoke(result);
    }
    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            TimeRemaining -= Time.deltaTime;
            float progress = 1 - TimeRemaining / maxTime;
            if (ProgressBar != null)
            {
                ProgressBar.fillAmount = progress;
            }

            //Tutorial
            if (isInTutorial)
            {
                if(PauseAtSuccess && progress >= successWindowStart && progress <= successWindowEnd)
                {
                    PauseAtSuccess = false;
                    PauseForTutorial();
                }
                else if(PauseAtPerfect && progress >= perfectWindowStart && progress <= perfectWindowEnd)
                {
                    PauseAtPerfect = false;
                    PauseForTutorial();
                }
            }

            if (TimeRemaining <= 0)
            {
                ResumeFromTutorialPause();
                EndQuickTimeEvent(QuickTimeEventResult.Failed);
            }

            if(Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (isInTutorial)
                {
                    ResumeFromTutorialPause();

                    if (PauseAtPerfect)
                    {
                        if (progress >= perfectWindowStart && progress <= perfectWindowEnd)
                        {
                            AudioManager.Instance.PlaySFX("parried", 0.4f);
                            EndQuickTimeEvent(QuickTimeEventResult.Perfect);
                        }

                        return;
                    }

                    if (PauseAtSuccess)
                    {
                        if (progress >= successWindowStart && progress <= successWindowEnd)
                        {
                            AudioManager.Instance.PlaySFX("blocked", 0.4f);
                            EndQuickTimeEvent(QuickTimeEventResult.Success);
                        }

                        return;
                    }
                }

                //Outside Tutorial
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
                    if(!isInTutorial) 
                        EndQuickTimeEvent(QuickTimeEventResult.Failed);
                }
            }
        }
    }
    void PauseForTutorial()
    {
        Time.timeScale = 0f;
    }

    void ResumeFromTutorialPause()
    {
        Time.timeScale = 1f;
    }
}
