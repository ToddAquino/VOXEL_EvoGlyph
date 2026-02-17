using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class AIController : MonoBehaviour, IUnitController
{
    public GameObject qteObj;
    public QuickTimeEventHandler qteHandler;
    //public AIAction[] AvailableActions;
    public CastSpellAction ActionToPerform;
    public bool isInTutorial = false;
    QuickTimeEventResult qteResult = QuickTimeEventResult.None;
    public void OnEndTurn(Unit unit, BattlePhase phase)
    {
        if (BattleManager.Instance == null) return;
        if (!isInTutorial)
        {
            switch (phase)
            {
                case BattlePhase.EnemyAction:
                    BattleManager.Instance.Controller.EndEnemyActionPhase();
                    break;
            }
        }
    }

    public void OnStartTurn(Unit unit, BattlePhase phase)
    {
        if (phase != BattlePhase.EnemyAction) return;
        StartCoroutine(EnemyActionRoutine(unit));
        //switch (phase)
        //{
        //    case BattlePhase.EnemyAction:
        //        Debug.Log($"Awaiting Action");
        //        StartCoroutine(PickAction(unit));
        //        qteHandler.StartCounterPhase();
        //        DoActionToPerform(unit);
        //        break;
        //}
    }

    IEnumerator EnemyActionRoutine(Unit unit)
    {
        //PickAction();
        ActionToPerform.IsCastingFinished = false;
        ActionToPerform.IsSpellResolved = false;
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("BeginCasting");

        yield return new WaitUntil(() => ActionToPerform.IsCastingFinished);

        yield return new WaitUntil(() => ActionToPerform.IsSpellResolved);

        yield return new WaitForSeconds(1.5f);
        unit.EndTurn(BattlePhase.EnemyAction);
    }
    public void OnCastStarted()
    {
        float windUpDuration = GetCurrentAnimationLength();
        qteObj.SetActive(true);
        qteHandler.StartQuickTimeEvent(windUpDuration);
        qteHandler.OnQTEFinished += HandleQTEResult;
    }

    public void HandleQTEResult(QuickTimeEventResult result)
    {
        qteHandler.OnQTEFinished -= HandleQTEResult;
        qteResult = result;

        ActionToPerform.SetQTEResult(qteResult);
        ActionToPerform.ReleaseSpell(this.GetComponent<Unit>());
        qteObj.SetActive(false);
    }
    //void PickAction()
    //{
    //    if (AvailableActions.Length == 0) yield break;

    //    int index = Random.Range(0,AvailableActions.Length);
    //    ActionToPerform = AvailableActions[index];

    //    ////Choose Target
    //    //TargetingController targeting = unit.TargetingController;
    //    //targeting.BeginTargetSelection(ActionToPerform.targetType);

    //    ////For Now Picks first available target (Can be more complex later on)

    //    //yield return new WaitForSeconds(1f);
    //    //if (targeting.currentValidTargets == null || targeting.currentValidTargets.Count == 0)
    //    //    yield break;
    //    //targeting.SelectTarget(targeting.currentValidTargets[0]);

    //    //unit.EndTurn(BattlePhase.EnemyPlanning);
    //}
    float GetCurrentAnimationLength()
    {
        Animator animator = GetComponent<Animator>();
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
}