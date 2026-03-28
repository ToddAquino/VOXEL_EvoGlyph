using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class RoomCameraManager : MonoBehaviour
{
    public static RoomCameraManager Instance;
    public List<CinemachineCamera> allRoomCameras = new List<CinemachineCamera>();
    private CinemachineCamera currentActiveCamera;
    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        foreach (var cam in allRoomCameras)
        {
            if (cam != null)
                cam.Priority = 10;
        }
    }
    public void SwitchToCamera(CinemachineCamera newCamera)
    {
        if (newCamera == null || newCamera == currentActiveCamera)
            return;

        if (currentActiveCamera != null)
        {
            currentActiveCamera.Priority = 10;
            currentActiveCamera.gameObject.SetActive(false);
        }

        newCamera.Priority = 30;
        newCamera.gameObject.SetActive(true);
        currentActiveCamera = newCamera;
    }
}
