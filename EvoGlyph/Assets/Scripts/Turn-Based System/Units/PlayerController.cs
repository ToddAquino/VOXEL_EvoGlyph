using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IUnitController
{
    public GlyphController controller;
    public void OnEndTurn(Unit unit)
    {
        controller.CanDrawGlyph(false);

        GlyphController.OnCreateGlyph -= ComparePattern;
    }

    public void OnStartTurn(Unit unit)
    {
        controller.Initialize();
        controller.CanDrawGlyph(true);
        GlyphController.OnCreateGlyph += ComparePattern;
    }

    void ComparePattern(List<int> Sequence)
    {
        foreach (var glyphs in GameManager.Instance.GlyphDatabase.ExistingGlyphs)
        {
            foreach (var sequences in glyphs.GlyphData.PatternPossibilities)
            {
                if (Sequence.SequenceEqual(sequences.GlyphPattern))
                {
                    Debug.Log($"<color=yellow>Match Found: {glyphs} was formed</color>");

                    ////Has to be unlocked
                    //if (GameManager.Instance.PlayerGlyphs.IsUnlocked(glyphs))
                    //{
                    //    //OnGlyphCast?.Invoke(glyphs);
                    //    glyphs.Activate();
                    //    return;
                    //}

                    //No longer locked behind progression
                    glyphs.Activate(this.GetComponent<Unit>());
                    return;
                }
            }
        }
    }
}
