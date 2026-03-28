using UnityEngine;

public class FixedRoom : RoomController
{
    [SerializeField] private RoomContent content;
    public override void Initialize()
    {
        if (content != null)
        {
            content.Initialize();
        }
    }
}
