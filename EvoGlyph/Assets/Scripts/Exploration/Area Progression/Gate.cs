using UnityEngine;

public class Gate : MonoBehaviour
{
    public GateKey gateKey;
    public bool isUnlocked = false;

    public void Initialize(bool unlockState)
    {
        isUnlocked = unlockState;
        HandleGateState();
    }
    public void UnlockGate(GateKey key)
    {
        if (isUnlocked) return;
        if (key == gateKey)
        {
            isUnlocked = true;
            HandleGateState();
        }
    }

    void HandleGateState()
    {
        if (isUnlocked)
        {
            gateKey.gameObject.SetActive(false);
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            gateKey.gameObject.SetActive(true);
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
    public string GetGateID()
    {
        return this.gameObject.name;
    }
}
