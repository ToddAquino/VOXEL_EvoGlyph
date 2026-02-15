using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DefendAction", menuName = "Action/AI/DefendAction")]
public class DefendAction : AIAction
{
    public override void Activate(Unit user)
    {
        base.Activate(user);
        user.StartCoroutine(PerformAction(user));
    }

    private IEnumerator PerformAction(Unit user)
    {
        user.GetTarget().HealthComponent.ActivateShield();
        yield return new WaitForSeconds(1f);
        user.EndTurn(BattlePhase.EnemyAction);
    }
}

