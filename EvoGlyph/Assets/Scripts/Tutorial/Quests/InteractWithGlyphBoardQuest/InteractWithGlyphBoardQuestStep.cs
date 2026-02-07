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

    private void GlyphBoardInteracted(List<int> glyph)
    {
        if(glyph == null) return;

            if (glyph.Count > 1)
            {
                FinishQuestStep();
                return;
            }
    }
}
