using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class AIController : MonoBehaviour, IUnitController
{
    public Image AIActionToPerformIcon;
    public AIAction[] AvailableActions;
    public AIAction ActionToPerform;
    public bool isInTutorial = false;
    public void OnEndTurn(Unit unit, BattlePhase phase)
    {
        if (BattleManager.Instance == null) return;
        if (!isInTutorial)
        {
            switch (phase)
            {
                case BattlePhase.EnemyPlanning:
                    BattleManager.Instance.Controller.EndEnemyPlanningPhase();
                    break;

                case BattlePhase.EnemyAction:
                    BattleManager.Instance.Controller.EndEnemyActionPhase();
                    break;
            }
        }
    }

    public void OnStartTurn(Unit unit, BattlePhase phase)
    {

        switch (phase)
        {
            case BattlePhase.EnemyPlanning:
                if(ActionToPerform == null)
                {
                    Debug.Log($"Awaiting Action");
                    StartCoroutine(PickAction(unit));
                }
                break;
            case BattlePhase.EnemyAction:
                if (ActionToPerform != null)
                {
                    HideActionChosen();
                    DoActionToPerform(unit);
                    ActionToPerform = null;
                }
                break;
        }
    }

    IEnumerator PickAction(Unit unit)
    {
        if (AvailableActions.Length == 0) yield break;
        int index = Random.Range(0,AvailableActions.Length-1);
        ActionToPerform = AvailableActions[index];
        DisplayActionChosen(ActionToPerform.Icon);

        //Choose Target
        TargetingController targeting = unit.TargetingController;
        targeting.BeginTargetSelection(ActionToPerform.targetType);
        //For Now Picks first available target (Can be more complex later on)

        yield return new WaitForSeconds(1f);
        targeting.SelectTarget(targeting.currentValidTargets[0]);

        unit.EndTurn(BattlePhase.EnemyPlanning);
    }

    public void DoActionToPerform(Unit unit)
    {
        ActionToPerform.Activate(unit);
    }
    public void DisplayActionChosen(Sprite icon)
    {
        AIActionToPerformIcon.sprite = icon;
        AIActionToPerformIcon.enabled = true;
    }

    public void HideActionChosen()
    {
        AIActionToPerformIcon.enabled = false;
    }
}