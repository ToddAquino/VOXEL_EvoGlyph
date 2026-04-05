using UnityEngine;

public class TomeMinigame : MonoBehaviour
{
    public bool isActive = false;
    TomeTower tomeTower;
    [SerializeField] GameObject pointer;
    [SerializeField] TomeBoard board;
    public void Initialize(TomeTower tower)
    {
        if (isActive) return;
        tomeTower = tower;
        board.IsInTutorial = tower.IsInTutorial;
        isActive = true;
        gameObject.SetActive(true);
    }

    public void BeginMinigame()
    {
        board.OnFinished += PathCompleted;
        board.StartMinigame(tomeTower.spellToUnlock);
    }

    void PathCompleted()
    {
        Debug.Log("Path Completed");
        tomeTower.UnlockSpell();
    }

    public void ExitMinigame()
    {
        board.OnFinished -= PathCompleted;

        isActive = false;
        if (tomeTower != null)
            tomeTower.SetPlayerCanMove();

        UIManager.Instance.ShowExplorationUI();
        gameObject.SetActive(false);
    }
}
