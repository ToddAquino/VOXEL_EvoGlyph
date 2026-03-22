using UnityEngine;

public class SetAreaOnTrigger : MonoBehaviour
{
    [SerializeField] int areaIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.ExplorationData.currentAreaIndex = areaIndex;
    }
}
