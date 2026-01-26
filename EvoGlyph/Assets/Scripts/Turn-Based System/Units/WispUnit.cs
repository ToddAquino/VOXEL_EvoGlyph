using UnityEngine;

public class WispUnit : Unit
{
    public int Damage;
    protected override void DoStartTurn()
    {
        base.DoStartTurn();
        Attack();
    }

    private void Attack()
    {
        Debug.Log($"<color=yellow> {this.name} attacks");
        OnEndTurn();
    }

    void OnEndTurn()
    {
        TurnBaseSystem.Instance.EndTurn();
    }
}
