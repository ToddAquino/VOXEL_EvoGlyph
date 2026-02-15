using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingController : TargetingController
{
    [SerializeField] SpriteRenderer TargetingMask;
    public override void BeginTargetSelection(TargetType type)
    {
        base.BeginTargetSelection(type);
        //ShowTargetingMask();
    }

    public override void EndTargetSelection()
    {
        base.EndTargetSelection();
        //HideTargetingMask();
    }
    public void ShowTargetingMask()
    {
        List<Unit> unitToHide = new List<Unit>();
        List<Unit> unitToHighlight = new List<Unit>();
        TargetingMask.gameObject.SetActive(true);

        foreach (Unit unit in BattleManager.Instance.Controller.aliveUnits)
        {
            if (currentValidTargets.Contains(unit))
            {
                unitToHighlight.Add(unit);
            }
            else
            {
                unitToHide.Add(unit);
            }
        }

        foreach (Unit unit in unitToHide)
        {
            unit.characterSprite.sortingOrder = 0;
        }
        // Targeting Mask is at sorting order 1
        foreach (Unit unit in unitToHighlight)
        {
            unit.characterSprite.sortingOrder = 2;
        }
    }

    public void HideTargetingMask()
    {
        foreach (Unit unit in BattleManager.Instance.Controller.aliveUnits)
        {
            unit.characterSprite.sortingOrder = 0;
        }

        TargetingMask.gameObject.SetActive(false);
    }
}
