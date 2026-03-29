using UnityEngine;

public class Gate : MonoBehaviour
{
    //public GateKey gateKey;
    [SerializeField] GameObject doorLight;
    public bool isUnlocked = false;

    public void Initialize(bool unlockState)
    {
        doorLight.SetActive(false);
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
            ShowDoorLight();
        }
        else
        {
            //gateKey.gameObject.SetActive(true);
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    void ShowDoorLight()
    {
        if (doorLight != null)
        {
            doorLight.SetActive(true);
        }
    }
    public string GetGateID()
    {
        return this.gameObject.name;
    }
}
