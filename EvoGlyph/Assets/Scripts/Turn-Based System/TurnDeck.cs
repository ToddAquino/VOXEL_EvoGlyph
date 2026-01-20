using UnityEngine;

public class TurnDeck : MonoBehaviour
{
    public Unit Unit;
    public Sprite Icon;
    public string Name;
    public int TeamNumber;
    public float Speed;
    public bool IsUnitTurnActive;
    public bool IsDead = false;
    private void GenerateTurnDeck()
    {
        Name = Unit.GetName();
        Icon = Unit.GetIcon();
        TeamNumber = Unit.GetTeamNumber();
        Speed = Unit.GetSpeed();

        if(CheckDeckValidity())
        {
            Unit.SetTurnDeck(this);
        }
        else
        {
            Debug.Log($"Unit: {Unit} Is Not Set Properly!");
            Destroy(this);
        }
    }

    //Check if TurnDeck holds a valid unit
    bool CheckDeckValidity()
    {
        return !string.IsNullOrWhiteSpace(Name)
        && Icon != null
        && TeamNumber != 0
        && Speed != 0;
    }

    public void OnTurnStart()
    {
        ResetActionPoints();
        EnableTurnBaseInputs();
        IsUnitTurnActive = true;
    }

    public void OnTurnEnd()
    {
        DisableTurnBaseInputs();
        IsUnitTurnActive = false;
        TurnSequence.Instance.EndTurn();
    }

    public void OnUnitKilled()
    {
        IsDead = true;
        TurnSequence.Instance.CheckEndConditions();
    }


    public void EnableTurnBaseInputs()
    {

    }

    public void DisableTurnBaseInputs()
    {

    }

    public void ResetActionPoints()
    {

    }

    private void OnDestroy()
    {
        Unit.SetTurnDeck(null);
    }
}
