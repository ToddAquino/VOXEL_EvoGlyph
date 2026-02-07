using UnityEngine;

[CreateAssetMenu(fileName = "AIAction", menuName = "Action/AIAction")]
public class AIAction : ScriptableObject
{
    public Sprite Icon;
    public virtual void Activate(Unit user)
    {
        Debug.Log($"{user.name} performs {this.name}");
    }
}
