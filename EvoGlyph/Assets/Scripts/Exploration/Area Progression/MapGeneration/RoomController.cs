
using UnityEngine;

//public enum RoomType
//{
//    Spawn,
//    Normal,
//    Boss
//}
public enum Direction
{
    Up, Down, Left, Right
}
public abstract class RoomController : MonoBehaviour
{
    //public RoomType roomType;
    //public Vector2Int gridPosition;
    //public Direction direction;
    //public Transform doorUp, doorDown, doorLeft, doorRight;
    //public GameObject wallUp, wallDown, wallLeft, wallRight;
    //[Header("Content")]
    //[SerializeField] GameObject[] possibleContents;
    //public Transform contentParent;
    [SerializeField] bool playerVisited = false;
    [SerializeField] bool contentInitialized = false;
    public bool isRoomCleared = false;
    public Gate[] RoomGates;
    public void OnPlayerEnter(MovingPlayerController player)
    {
        if (!playerVisited)
        {
            playerVisited = true;
        }
        
    }
    public abstract void Initialize();

    //void InitializeRoomContent()
    //{
    //    if (contentInitialized) return;
    //    contentInitialized = true;
    //    if (possibleContents.Length == 0) return;
    //    if(existingContentObj != null)
    //    {
    //        Destroy(existingContentObj);
    //        Debug.Log($"Destroyed content ");
    //    }

    //    int index = Random.Range(0, possibleContents.Length);
    //    GameObject chosenContent = possibleContents[index];
    //    existingContentObj = Instantiate(chosenContent, contentParent.position, Quaternion.identity, contentParent);
    //    Debug.Log($"Length: {possibleContents.Length} ChosenIndx: {index} ");
    //}
    //void ActivateRoomContent()
    //{
    //    if (isRandom)
    //        InitializeRoomContent();
    //}

    //public void OpenDoor(Direction dir)
    //{
    //    switch (dir)
    //    {
    //        case Direction.Up: 
    //            if (wallUp)
    //            {
    //                wallUp.SetActive(false); 
    //            }
    //            break;

    //        case Direction.Down:
    //            if (wallDown)
    //            {
    //                wallDown.SetActive(false);
    //            }
    //            break;
            
    //        case Direction.Left:
    //            if (wallLeft)
    //            {
    //                wallLeft.SetActive(false);
    //            }
    //            break;
    //        case Direction.Right:
    //            if (wallRight)
    //            {
    //                wallRight.SetActive(false);
    //            }
    //            break;
    //    }
    //}
}

