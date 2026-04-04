using UnityEngine;

public class TomeRoom : RoomController
{
    [SerializeField] TomeTower tomeTower;
    [SerializeField] bool isInitialized = false;

    private void OnEnable()
    {
        tomeTower.OnUnlock += CheckIfRoomCleared;
    }

    private void OnDisable()
    {
        tomeTower.OnUnlock -= CheckIfRoomCleared;
    }
    public override void Initialize()
    {
        if (isInitialized) return;

        isInitialized = true;

        InitializeTome();
        CheckIfRoomCleared();
        InitializeGates();
    }

    void InitializeTome()
    {
        tomeTower.Initialize();
    }

    void CheckIfRoomCleared()
    {
        if (tomeTower.IsUnlocked)
        {
            isRoomCleared = true;
        }
        InitializeGates();
    }
    void InitializeGates()
    {
        foreach (Gate gate in RoomGates)
        {
            gate.Initialize(isRoomCleared);
        }
    }
}
