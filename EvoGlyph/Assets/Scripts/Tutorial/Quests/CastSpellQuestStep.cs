using UnityEngine;
using System.Collections.Generic;
public class CastSpellQuestStep : QuestStep
{
    [SerializeField] BattleManager battleManager;
    protected override void EnableStep()
    {
        PlayerUnit player = battleManager.playerUnit;
        if (player != null)
            player.GetComponent<PlayerController>().glyphSequencer.OnEndSequence += (CastFinished);
    }
    protected override void DisableStep()
    {
        PlayerUnit player = battleManager.playerUnit;
        if (player != null)
            player.GetComponent<PlayerController>().glyphSequencer.OnEndSequence -= (CastFinished);
    }
    private void CastFinished(List<Glyph> glyph)
    {
        FinishQuestStep();
    }
}
