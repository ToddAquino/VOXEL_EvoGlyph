using System.Collections.Generic;
using UnityEngine;

public class PickupRoom : RoomController
{
    [SerializeField] List<TomePickup> existingPickups;
    private void OnEnable()
    {
        TomePickup.OnTomePickedUp += HandleTomePickedUp;
    }

    private void OnDisable()
    {
        TomePickup.OnTomePickedUp -= HandleTomePickedUp;
    }
    public override void Initialize()
    {
        CheckIfRoomCleared();
        InitializeTomes();
        InitializeGates();
    }
        
    void InitializeTomes()
    {
        foreach (TomePickup pickup in existingPickups)
        {
            if(!GameManager.Instance.ExplorationData.IsTomeLooted(pickup.GetTomeID()))
            {
                pickup.Initialize();
            }
        }
    }
    void InitializeGates()
    {
        foreach (Gate gate in RoomGates)
        {
            gate.Initialize(isRoomCleared);
        }
    }
    void HandleTomePickedUp(string tomeID)
    {
        Debug.Log("Cheking");
        CheckIfRoomCleared();
    }

    void CheckIfRoomCleared()
    {
        if (existingPickups.Count == 0) return;

        foreach (var pickup in existingPickups)
        {

            if (!GameManager.Instance.ExplorationData.IsTomeLooted(pickup.GetTomeID()))
            {
                Debug.Log("AreaNotCleared");
                return;
            }
        }
        isRoomCleared = true;
        foreach (Gate gate in RoomGates)
        {
            gate.UnlockGate();
        }
    }
}
