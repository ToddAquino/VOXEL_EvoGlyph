using System;
using UnityEngine;

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

    public void SetState(GameState newState)
    {
        CurrentGameState = newState;
        OnGameStateChanged?.Invoke(newState);
    }
}
