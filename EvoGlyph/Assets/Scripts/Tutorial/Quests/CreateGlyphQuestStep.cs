using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CreateGlyphQuestStep : QuestStep
{
   //[SerializeField] GlyphController controller;
    [SerializeField] Glyph requiredGlyph;
    //[SerializeField] int maxTries = 3;
    //[SerializeField] int tryCount;
    protected override void EnableStep()
    {
        GlyphController.OnCreateGlyph += GlyphCreated;
        //tryCount = maxTries;
    }

    protected override void DisableStep()
    {
        GlyphController.OnCreateGlyph -= GlyphCreated;
    }

    private void GlyphCreated(bool[] glyph)
    {
        if (glyph == null)
        {
            Debug.Log("glyph is null");
            return;
        }

        if (glyph.SequenceEqual(requiredGlyph.pattern.glyphPattern))
        {
            Debug.Log("Create glyph Success");
            FinishQuestStep();
            return;
        }
        Debug.Log("Create glyph Failed");
        //else
        //{
        //    tryCount--;

        //    if (tryCount <= 0)
        //    {
        //        Debug.Log("Create glyph Failed");
        //        GlyphFailed();
        //    }
        //}
    }
    private void GlyphFailed()
    {
        FailedQuestStep();
    }
}
