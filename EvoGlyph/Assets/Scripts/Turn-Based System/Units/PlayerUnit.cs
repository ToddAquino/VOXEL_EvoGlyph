using UnityEngine;

public class PlayerUnit : Unit
{

    protected override void DoStartTurn()
    {
        base.DoStartTurn();
        GlyphBoard.Instance.CanInteract = true;
    }

    public void OnEndTurn()
    {
        GlyphBoard.Instance.CanInteract = false;
        TurnBaseSystem.Instance.EndTurn();
    }
}
