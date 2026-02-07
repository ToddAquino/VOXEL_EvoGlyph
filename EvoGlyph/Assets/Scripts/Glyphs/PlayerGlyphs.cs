using System.Collections.Generic;
using UnityEngine;

public class PlayerGlyphs : MonoBehaviour
{
    private List<Glyph> unlockedGlyphList = new List<Glyph>();
    public bool IsUnlocked(Glyph glyph)
    {
        return unlockedGlyphList.Contains(glyph);
    }
    public void UnlockGlyph(Glyph glyph)
    {
        if(!unlockedGlyphList.Contains(glyph))
            unlockedGlyphList.Add(glyph);
    }
}
