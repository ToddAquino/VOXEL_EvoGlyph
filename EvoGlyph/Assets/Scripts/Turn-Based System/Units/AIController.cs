using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class AIController : MonoBehaviour, IUnitController
{
    public Image AIActionToPerformIcon;
    public AIAction[] AvailableActions;
    public AIAction ActionToPerform;
    public void OnEndTurn(Unit unit)
    {
        
    }

    public void OnStartTurn(Unit unit)
    {
        if(ActionToPerform != null)
        {
            HideActionChosen();
            DoActionToPerform(unit);
            ActionToPerform = null;
        }
        else
        {
            Debug.Log($"Awaiting Action");
            StartCoroutine(PickAction(unit));
        }
    }

    IEnumerator PickAction(Unit unit)
    {
        if (AvailableActions.Length == 0) yield break;
        Debug.Log($"Picking Action {AvailableActions.Length}");
        int index = Random.Range(0,AvailableActions.Length-1);
        ActionToPerform = AvailableActions[index];
        DisplayActionChosen(ActionToPerform.Icon);
        yield return new WaitForSeconds(1f);

        unit.EndTurn();
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