using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackAction", menuName = "Action/AI/AttackAction")]
public class AttackAction : AIAction
{
    public int Damage;
    public override void Activate(Unit user)
    {
        base.Activate(user);
        user.StartCoroutine(PerformAction(user));
    }

    private IEnumerator PerformAction(Unit user)
    {
        user.GetTarget().HealthComponent.TakeDamage(Damage);
        yield return new WaitForSeconds(1f);
        user.EndTurn(BattlePhase.EnemyAction);
    }
}
