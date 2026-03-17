using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlyphDatabase : MonoBehaviour
{
    [Header("Glyph Library")]
    public List<Glyph> ExistingGlyphs;

    public bool TryGetValidGlyphFromPattern(bool[] sequence)
    {
        foreach (var glyphs in ExistingGlyphs)
        {
            if (sequence.SequenceEqual(glyphs.pattern.glyphPattern))
            {
                return true;
            }
        }
        return false;
    }
    public Glyph GetGlyphFromPattern(bool[] pattern)
    {
        foreach (var glyph in ExistingGlyphs)
        {
            if (pattern.SequenceEqual(glyph.pattern.glyphPattern))
            {
                return glyph;
            }
        }

        return null;
    }
}
