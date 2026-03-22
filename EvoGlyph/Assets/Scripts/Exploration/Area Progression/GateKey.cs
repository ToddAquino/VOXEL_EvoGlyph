using System;
using UnityEngine;

public class GateKey : MonoBehaviour
{
    public event Action<GateKey> OnUnlock;

    public void Unlock()
    {
        OnUnlock?.Invoke(this);
    }
}
