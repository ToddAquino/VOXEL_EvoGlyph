using Unity.Cinemachine;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [Header("Room")]
    public RoomController myRoom;

    [Header("Camera")]
    [SerializeField] CinemachineCamera roomCam;
    [SerializeField] int activePriority = 20;
    [SerializeField] int inactivePriority = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Move player slightly into next room (or just let them walk since one scene)
            if (roomCam != null)
            {
                RoomCameraManager.Instance.SwitchToCamera(roomCam);
            }
            if (myRoom != null)
            {
                myRoom.OnPlayerEnter(other.GetComponent<MovingPlayerController>());
            }
        }
    }
}
