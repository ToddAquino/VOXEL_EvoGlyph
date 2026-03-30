using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicAttackQTE : MonoBehaviour
{
    public event Action<QuickTimeEventResult> OnQTEFinished;

    [SerializeField] GameObject SuccessWindow;
    [SerializeField] Transform edgeLeft;
    [SerializeField] Transform edgeRight;

    public int direction = 1;
    public float speed = 5f;
    [SerializeField] bool willSucceed = false;
    public bool IsActive;
    public bool IsInTutorial;
    [Header("QTE Settings")]
    [SerializeField] float successWindowSize = 5f;

    public void StartQuickTimeEvent(bool inTutorial)
    {
        IsInTutorial = inTutorial;
        SetSuccessWindowPosition();
        IsActive = true;
    }

    public void SetSuccessWindowPosition()
    {
        float minX = edgeLeft.position.x;
        float maxX = edgeRight.position.x;
        float randXPos;
        if (IsInTutorial)
        {
            randXPos = maxX;
        }
        else
        {
            randXPos = UnityEngine.Random.Range(minX, maxX);
        }
        SuccessWindow.transform.position = new Vector3(randXPos, this.transform.position.y, this.transform.position.z);
    }

    void Update()
    {
        if (IsActive)
        {
            transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

            if (transform.position.x >= edgeRight.position.x)
            {
                direction = -1;
            }
            else if (transform.position.x <= edgeLeft.position.x)
            {
                direction = 1;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (IsInTutorial)
                {
                    if (willSucceed)
                    {
                        Time.timeScale = 1f;
                    }
                    else
                    {
                        return;
                    }
                }
                IsActive = false;
                EndQuickTimeEvent(GetResult());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == SuccessWindow)
        {
            willSucceed = true;
            if (IsInTutorial)
            {
                //stop time to guarantee success chance
                Time.timeScale = 0f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == SuccessWindow)
        {
            willSucceed = false;
        }
    }

    QuickTimeEventResult GetResult()
    {
        if (willSucceed)
        {
            return QuickTimeEventResult.Success;
        }
        else
        {
            return QuickTimeEventResult.Failed;
        }
    }

    void EndQuickTimeEvent(QuickTimeEventResult result)
    {
        Time.timeScale = 1f;
        OnQTEFinished?.Invoke(result);
    }
}
