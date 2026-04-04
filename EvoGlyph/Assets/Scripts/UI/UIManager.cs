using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public ExplorationUIController ExplorationUIController;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    public void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Exploration:
                ShowExplorationUI();
                break;

            case GameState.Battle:
                ShowBattleUI();
                break;
        }
    }

    public void ShowExplorationUI()
    {
        ExplorationUIController.Initialize();
    }

    public void ShowBattleUI()
    {
        ExplorationUIController.DeInitialize();
    }
}
