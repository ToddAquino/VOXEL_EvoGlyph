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

    [Header("QTE Settings")]
    [SerializeField] float successWindowSize = 5f;

    public void StartQuickTimeEvent()
    {
        SetSuccessWindowPosition();
        IsActive = true;
    }

    public void SetSuccessWindowPosition()
    {
        float minX = edgeLeft.position.x;
        float maxX = edgeRight.position.x;
        float randXPos = UnityEngine.Random.Range(minX, maxX);
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
       
        OnQTEFinished?.Invoke(result);
    }
}
