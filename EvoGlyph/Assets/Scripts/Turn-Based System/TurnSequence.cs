using  System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnSequence : MonoBehaviour
{
    private static TurnSequence instance;
    public static TurnSequence Instance { get { return instance; } }

    public List<TurnDeck> Decks = new List<TurnDeck>();
    public int TurnIndex = 0;
    public TurnDeck currentActiveTurnDeck; //The current unit that is active and doing its turn

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("WARNING Multiple TurnSequence Instance Exists!");
        }
        else
        {
            instance = this;
        }
    }

    // Cycle/Phase Scripts
    public void BeginTurnBaseSystem() //Starts Turn-Based game cycle
    {
        TurnIndex = 0;
        StartNextTurn();
    }

    public void StartNextTurn()
    {
        if (Decks.Count == 0)
            return;

        currentActiveTurnDeck = Decks[TurnIndex % Decks.Count];
        currentActiveTurnDeck.OnTurnStart();
    }

    public void EndTurn()
    {
        currentActiveTurnDeck.OnTurnEnd();
        TurnIndex++;

        StartNextTurn();
    }

    // Turn Deck List Manipulation Scripts
    public void AddToDecksList(TurnDeck deck)
    {
        Decks.Add(deck);
    }

    public void CheckEndConditions()
    {
        // Remove dead units in pool
        Decks.RemoveAll(d => d.IsDead);
        
        if(GetAliveTeamsCount() <= 1)
        {
            EndTurnBaseSystem();
        }
    }

    private int GetAliveTeamsCount()
    {
        List<int> aliveTeams = new List<int>();

        foreach (var deck in Decks)
        {
            if (deck.TeamNumber != 0 && !aliveTeams.Contains(deck.TeamNumber))
            {
                aliveTeams.Add(deck.TeamNumber);
            }
        }

        return aliveTeams.Count;
    }

    private void EndTurnBaseSystem() // Ends Turn base battle
    {
        List<TurnDeck> decksToDestroy = new List<TurnDeck>(Decks);
        Decks.Clear();
        foreach (var deck in decksToDestroy)
        {
            Destroy(deck.gameObject);
        }
    }
}
