using System.Collections.Generic;
using UnityEngine;

public class AreaHandler : MonoBehaviour
{
    public ElementType AreaType;
    [SerializeField] string SceneName;
    [SerializeField] List<RoomController> allRooms = new List<RoomController>();
    [SerializeField] bool isFirstLoad = true;
    [SerializeField] SpawnHandler spawnHandler;
    private void Awake()
    {
        allRooms.AddRange(FindObjectsByType<RoomController>(FindObjectsSortMode.None));
    }
    void Start()
    {
        GameManager.Instance.ExplorationData.CurrentAreaType = AreaType;
        GameManager.Instance.SetState(GameState.Exploration);
        if(GameManager.Instance.ExplorationData.currentExplorationScene == SceneName)
        {
            isFirstLoad = false;

            spawnHandler.MoveToSpawnPoint(GameManager.Instance.ExplorationData.GetPlayerPosition());
        }
        else
        {
            isFirstLoad = true;
            spawnHandler.MoveToInitialSpawn();
            TrackCurrentScene();
        }
        GenerateRooms();
    }

    void GenerateRooms()
    {
        foreach (RoomController room in allRooms)
        {
            if (room != null)
            {
                RandomRoom randomRoom = room as RandomRoom;
                if(isFirstLoad && randomRoom != null)
                {
                    randomRoom.isRandomized = false;
                }
                else if (!isFirstLoad && randomRoom != null)
                {
                    randomRoom.isRandomized = true;
                }
                room.Initialize();
            }
        }
    }

    void TrackCurrentScene()
    {
        GameManager.Instance.ExplorationData.currentExplorationScene = SceneName;
    }
}
