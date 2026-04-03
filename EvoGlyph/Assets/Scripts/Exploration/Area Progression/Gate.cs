using UnityEngine;

public class Gate : MonoBehaviour
{
    //public GateKey gateKey;
    [SerializeField] GameObject arrow;
    public bool isUnlocked = false;

    public void Initialize(bool unlockState)
    {
        arrow.SetActive(false);
        isUnlocked = unlockState;
        HandleGateState();
    }
    public void UnlockGate()
    {
        if (isUnlocked) return;
        //if (key == gateKey)
        //{
            isUnlocked = true;
            HandleGateState();
        //}
    }

    void HandleGateState()
    {
        if (isUnlocked)
        {
            //gateKey.gameObject.SetActive(false);
            this.GetComponent<BoxCollider2D>().enabled = false;
            ShowArrow();
        }
        else
        {
            //gateKey.gameObject.SetActive(true);
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    void ShowArrow()
    {
        if (arrow != null)
        {
            arrow.SetActive(true);
        }
    }
    public string GetGateID()
    {
        return this.gameObject.name;
    }
}
