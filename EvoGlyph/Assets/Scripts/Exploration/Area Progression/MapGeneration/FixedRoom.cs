using UnityEngine;

public class FixedRoom : RoomController
{
    public override void Initialize()
    {
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
