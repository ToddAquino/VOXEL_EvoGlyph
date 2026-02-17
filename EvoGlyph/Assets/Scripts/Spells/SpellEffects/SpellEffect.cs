using System;
using UnityEngine;

public abstract class SpellEffect : MonoBehaviour
{
    public event Action<SpellEffect> OnEffectFinished;
    protected void EffectSuccessfullyApplied()
    {
        OnEffectFinished?.Invoke(this);
    }
    public abstract void Apply(GameObject target);
}
