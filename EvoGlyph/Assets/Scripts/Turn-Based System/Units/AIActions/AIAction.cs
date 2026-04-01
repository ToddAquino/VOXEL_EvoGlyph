using System;
using UnityEngine;

public class AIAction : MonoBehaviour
{
    public event Action OnActionResolved;

    protected QuickTimeEventResult qteResult;
    public void SetQTEResult(QuickTimeEventResult result)
    {
        qteResult = result;
    }

    public virtual void DoAction(Unit user)
    {

    }

    public void HandleActionResolved()
    {
        OnActionResolved?.Invoke();
    }
}
