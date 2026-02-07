using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WaitAction", menuName = "Action/AI/WaitAction")]
public class WaitAction : AIAction
{
    public override void Activate(Unit user)
    {
        base.Activate(user);
        user.StartCoroutine(PerformAction(user));
    }

    private IEnumerator PerformAction(Unit user)
    {
        //Do Idle Animation
        yield return new WaitForSeconds(1f);
        user.EndTurn();
    }
}
