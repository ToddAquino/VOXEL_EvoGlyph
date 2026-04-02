using UnityEngine;

public class ManaTowerTutorialLoader : MonoBehaviour,IInteractable
{
    [SerializeField] Tutorial tutorial;
    [SerializeField] ManaTower tower;
    public void Interact(MovingPlayerController player)
    {
        tutorial.gameObject.SetActive(true);
        tower.Interact(player);
    }
}
