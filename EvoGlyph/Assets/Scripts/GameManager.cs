using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Exploration,
    Battle
}

public class GameManager : MonoBehaviour
{
    public static Action<GameState> OnGameStateChanged;
    public static GameManager Instance;
    //TutorialManager tutorialManager;
    //[SerializeField] Tutorial[] tutorialQuests;
    //[SerializeField] int tutorialQuestIndex;
    public GameState CurrentGameState;
    public bool LoadMainMenuOnStart = true; //Debug
    public ElementalSynergyDatabase ElementalSynergyDatabase;
    public GlyphDatabase GlyphDatabase;
    public PlayerData PlayerData;
    public ExplorationData ExplorationData;
    public ElementHandler ElementHandler;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if(LoadMainMenuOnStart)
            SceneManager.LoadSceneAsync("MainMenu");
    }

    public void SetState(GameState newState)
    {
        CurrentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }
    public void IncendiumDeleteAllPrefs()
    {
        //deletes all saves per play, to be used on Play button at main menu
        PlayerPrefs.DeleteAll();
    }
}
