using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class AIController : MonoBehaviour, IUnitController
{
    EnemyUnit enemy;
    public GameObject qteObj;

    [Header("QTE")]
    public QuickTimeEventHandler qteHandler;
    public bool IsBeingParried = false;

    [Header("Spell System")]
    public CastSpellAction ActionToPerform;
    public AnimationClip castingAnimation;
    [SerializeField] Transform spellSpawnAnchor;

    public bool isInTutorial = false;
    QuickTimeEventResult qteResult = QuickTimeEventResult.None;

    private void Awake()
    {
        enemy = GetComponent<EnemyUnit>();
    }
    public void OnEndTurn()
    {
        if (BattleManager.Instance == null) return;

        BattleManager.Instance.Controller.EndEnemyActionPhase();
    }

    public void OnStartTurn()
    {
        BeginEnemyAction();
    }

    void BeginEnemyAction()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("BeginCasting");
        OnCastStarted();
    }
    void OnCastStarted()
    {
        if(IsBeingParried)
        {
            float windUpDuration = castingAnimation.length;
            qteObj.SetActive(true);
            qteHandler.StartQuickTimeEvent(windUpDuration);
            qteHandler.OnQTEFinished += HandleQTEResult;
            IsBeingParried = false;
        }
        else
        {
            HandleQTEResult(QuickTimeEventResult.Failed);
        }
    }

    void HandleQTEResult(QuickTimeEventResult result)
    {
        qteHandler.OnQTEFinished -= HandleQTEResult;
        qteResult = result;
        ActionToPerform.OnActionResolved += HandleActionFinished;
        ActionToPerform.SetQTEResult(qteResult);
        ActionToPerform.ReleaseSpell(enemy);
        qteObj.SetActive(false);
    }
    void HandleActionFinished()
    {
        ActionToPerform.OnActionResolved -= HandleActionFinished;
        if (!isInTutorial && enemy.HealthComponent.IsAlive)
            StartCoroutine(DoEndTurn());
        else if (!isInTutorial && !enemy.HealthComponent.IsAlive)
            OnEndTurn();

    }
    IEnumerator DoEndTurn()
    {
        yield return new WaitForSeconds(1.5f);
        enemy.EndTurn(BattlePhase.EnemyAction);
    }
    float GetCurrentAnimationLength()
    {
        Animator animator = GetComponent<Animator>();
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
}