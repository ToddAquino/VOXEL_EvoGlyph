using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IUnitController
{
    public GlyphController controller;
    private int callCount = 0;
    public void OnEndTurn(Unit unit)
    {
        controller.CanDrawGlyph(false);

        GlyphController.OnCreateGlyph -= ComparePattern;
    }

    public void OnStartTurn(Unit unit)
    {
        controller.Initialize();
        controller.CanDrawGlyph(true);
        //adding this for multiple games
        callCount = 0;
        GlyphController.OnCreateGlyph -= ComparePattern;
        GlyphController.OnCreateGlyph += ComparePattern;
    }

    void ComparePattern(bool[] Sequence)
    {
        
        foreach (var glyphs in GameManager.Instance.GlyphDatabase.ExistingGlyphs)
        {
            if (Sequence.SequenceEqual(glyphs.GlyphData.glyphPattern))
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
