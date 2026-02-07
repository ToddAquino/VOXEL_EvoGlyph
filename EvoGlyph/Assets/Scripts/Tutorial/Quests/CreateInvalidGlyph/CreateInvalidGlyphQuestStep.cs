using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class CreateInvalidGlyphQuestStep : QuestStep
{
    protected override void EnableStep()
    {
        GlyphController.OnCreateGlyph += GlyphCreated;
    }

    protected override void DisableStep()
    {
        GlyphController.OnCreateGlyph -= GlyphCreated;
    }

    private void GlyphCreated(List<int> glyph)
    {
        if (glyph == null) return;
        List<Glyph> glyphList = GameManager.Instance.GlyphDatabase.ExistingGlyphs;

        bool isValidGlyph = glyphList.Any(validGlyphs =>
        validGlyphs.GlyphData.PatternPossibilities.Any(pattern => 
        glyph.SequenceEqual(pattern.GlyphPattern)));

        if (!isValidGlyph && glyph.Count > 1)
        {
            FinishQuestStep();
            return;
        }
    }
}
