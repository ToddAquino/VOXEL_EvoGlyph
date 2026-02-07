using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlyphDatabase : MonoBehaviour
{
    [Header("Glyph Library")]
    public List<Glyph> ExistingGlyphs;

    public bool TryGetValidGlyphFromPattern(List<int> sequence)
    {
        foreach (var glyphs in GameManager.Instance.GlyphDatabase.ExistingGlyphs)
        {
            foreach (var pattern in glyphs.GlyphData.PatternPossibilities)
            {
                if (sequence.SequenceEqual(pattern.GlyphPattern))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
