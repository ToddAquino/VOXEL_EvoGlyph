using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class AIController : MonoBehaviour, IUnitController
{
    public GameObject qteObj;
    public QuickTimeEventHandler qteHandler;
    public CastSpellAction ActionToPerform;
    public bool isInTutorial = false;
    QuickTimeEventResult qteResult = QuickTimeEventResult.None;
    public void OnEndTurn()
    {
        if (BattleManager.Instance == null) return;
        if (!isInTutorial)
        {
            BattleManager.Instance.Controller.EndEnemyActionPhase();
        }
    }

    public void OnStartTurn()
    {
        StartCoroutine(EnemyActionRoutine(this.GetComponent<Unit>()));
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

    float GetCurrentAnimationLength()
    {
        Animator animator = GetComponent<Animator>();
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
}