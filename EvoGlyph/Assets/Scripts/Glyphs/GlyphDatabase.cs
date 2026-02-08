using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlyphDatabase : MonoBehaviour
{
    [Header("Glyph Library")]
    public List<Glyph> ExistingGlyphs;

    public bool TryGetValidGlyphFromPattern(bool[] sequence)
    {
        foreach (var glyphs in GameManager.Instance.GlyphDatabase.ExistingGlyphs)
        {
            if (sequence.SequenceEqual(glyphs.GlyphData.glyphPattern))
            {
                return true;
            }
        }
        return false;
    }
}
