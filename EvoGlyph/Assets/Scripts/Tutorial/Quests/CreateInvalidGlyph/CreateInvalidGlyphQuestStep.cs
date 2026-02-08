using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class CreateInvalidGlyphQuestStep : QuestStep
{
    int minActiveNodeCount = 2; //Number of nodes needed to be active to consider board interaction (2 nodes active forms a line pattern meaning interacted)
    protected override void EnableStep()
    {
        GlyphController.OnCreateGlyph += GlyphCreated;
    }

    protected override void DisableStep()
    {
        GlyphController.OnCreateGlyph -= GlyphCreated;
    }

    private void GlyphCreated(bool[] glyph)
    {
        if (glyph == null) return;
        //List<Glyph> glyphList = GameManager.Instance.GlyphDatabase.ExistingGlyphs;

        //bool isValidGlyph = glyphList.Any(validGlyphs =>
        //validGlyphs.GlyphData.PatternPossibilities.Any(pattern => 
        //glyph.SequenceEqual(pattern.GlyphPattern)));
        int ActiveNodesCount = 0;
        bool hasMultipleActiveNodes = false;
        foreach (var node in glyph)
        {
            if (node == true)
                ActiveNodesCount++;
            if (ActiveNodesCount >= minActiveNodeCount)
            {
                hasMultipleActiveNodes = true;
                break;
            }

        }
        if (!GameManager.Instance.GlyphDatabase.TryGetValidGlyphFromPattern(glyph) && hasMultipleActiveNodes ==  true)
        {
            FinishQuestStep();
            return;
        }
    }
}
