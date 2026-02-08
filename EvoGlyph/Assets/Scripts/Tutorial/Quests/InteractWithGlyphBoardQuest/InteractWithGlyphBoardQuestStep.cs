using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InteractWithGlyphBoardQuestStep : QuestStep
{

    protected override void EnableStep()
    {
        GlyphController.OnCreateGlyph += GlyphBoardInteracted;
    }

    protected override void DisableStep()
    {
        GlyphController.OnCreateGlyph -= GlyphBoardInteracted;
    }

    private void GlyphBoardInteracted(bool[] glyph)
    {
        if(glyph == null) return;
        int ActiveNodesCount = 0;
        foreach (var node in glyph)
        {
            if (node == true)
                ActiveNodesCount++;

        }
        if (ActiveNodesCount > 1)
        {
            FinishQuestStep();
        }
    }
}
