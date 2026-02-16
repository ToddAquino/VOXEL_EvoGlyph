using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CreateGlyphQuestStep : QuestStep
{
    [SerializeField] GlyphController controller;
    [SerializeField] Glyph requiredGlyph;

    protected override void EnableStep()
    {
        GlyphController.OnCreateGlyph += GlyphCreated;
        //controller.OnTimerRanOut.AddListener(GlyphFailed);
    }

    protected override void DisableStep()
    {
        GlyphController.OnCreateGlyph -= GlyphCreated;
        //controller.OnTimerRanOut.RemoveListener(GlyphFailed);
    }

    private void GlyphCreated(bool[] glyph)
    {
        if(glyph == null) return;

        if (glyph.SequenceEqual(requiredGlyph.GlyphData.glyphPattern))
        {
            FinishQuestStep();
            return;
        }
    }
    private void GlyphFailed()
    {
        FailedQuestStep();
    }
}
