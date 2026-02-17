using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AIAction", menuName = "Action/AIAction")]
public class AIAction : ScriptableObject
{
    public event Action<AIAction> OnActionFinished;
    public Sprite Icon;
    public TargetType targetType;

    protected void ActionSuccessfullyExecuted()
    {
        OnActionFinished?.Invoke(this);
    }
    public virtual void Activate(Unit user)
    {
        Debug.Log($"{user.name} performs {this.name}");
    }
}
