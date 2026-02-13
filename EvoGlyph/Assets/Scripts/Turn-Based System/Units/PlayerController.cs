using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IUnitController
{
    public GlyphController controller;
    public InventoryContainer sequencerContainer;
    public GameObject EndTurnButton;
    public GlyphSequencer glyphSequencer;
    public Glyph glyphToActivate;
    public Animator animator;
    private int callCount = 0;
    public void OnEndTurn(Unit unit)
    {
        controller.CanDrawGlyph(false);
        //reset glyph sequence
        if (EndTurnButton != null)
        {
            EndTurnButton.SetActive(false);
        }
        if (!glyphSequencer.SequencerContainer.slots[0].IsEmpty)
        {
            glyphSequencer.EndSequence();
        }
        GlyphController.OnCreateGlyph -= ComparePattern;
    }

    public void OnStartTurn(Unit unit)
    {
        controller.Initialize();
        if (EndTurnButton != null)
        {
            EndTurnButton.SetActive(true);
        }
        glyphSequencer.gameObject.SetActive(true);
        controller.CanDrawGlyph(true);
        if (controller.CanInteract == false)
        {
            controller.CanInteract = true;
        }
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
                callCount++;
                Debug.Log($"<color=yellow>Match Found: {glyphs} was formed, #{callCount} this turn</color>");

                ////Has to be unlocked
                //if (GameManager.Instance.PlayerGlyphs.IsUnlocked(glyphs))
                //{
                //    //OnGlyphCast?.Invoke(glyphs);
                //    glyphs.Activate();
                //    return;
                //}

                //No longer locked behind progression
                //glyphs.Activate(this.GetComponent<Unit>());
                if (!glyphSequencer.gameObject.activeSelf)
                {
                    glyphSequencer.gameObject.SetActive(true);
                }
                AddGlyphToSequencer(glyphs);
                return;
            }
        }
    }

    public void AddGlyphToSequencer(Glyph glyph)
    {
        if(sequencerContainer == null) return;
        sequencerContainer.AddItem(glyph);
    }

    public void AttackWithGlyph(Glyph glyph)
    {
        glyphToActivate = glyph;
        animator.SetTrigger("OnAttack");
    }
    public void DoAttack()
    {
        Unit caster = this.GetComponent<Unit>();
        if (glyphToActivate == null || caster == null) return;
        Spell spawnedSpell = glyphToActivate.Activate(caster);
        glyphSequencer.RegisterSpell(spawnedSpell);
    }
}
