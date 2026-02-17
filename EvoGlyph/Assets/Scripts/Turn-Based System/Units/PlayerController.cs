using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IUnitController
{
    public GlyphController controller;
    public InventoryContainer sequencerContainer;
    public GlyphSequencer glyphSequencer;
    public Glyph glyphToActivate;
    public Animator animator;
    private int callCount = 0;
    public bool isInTutorial = false;
    public void ListenToControllerInput()
    {
        GlyphController.OnCreateGlyph -= ComparePattern;
        GlyphController.OnCreateGlyph += ComparePattern;
    }

    public void StopListenToControllerInput()
    {
        GlyphController.OnCreateGlyph -= ComparePattern;
    }

    public void OnEndTurn()
    {
        controller.GlyphControllerOnEndTurn();
        //reset glyph sequence
        if (!glyphSequencer.SequencerContainer.slots[0].IsEmpty)
        {
            glyphSequencer.EndSequence();
        }
        glyphSequencer.gameObject.SetActive(false);
        StopListenToControllerInput();
        //Tutorial has control on end turn
        if (!isInTutorial)
        {
            BattleManager.Instance.Controller.EndPlayerActionPhase();
        }
    }

    public void OnStartTurn()
    {
        controller.Initialize();
   
        controller.CanDrawGlyph(true);
        glyphSequencer.gameObject.SetActive(true);
        ListenToControllerInput();


        //adding this for multiple games
        callCount = 0;
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
        glyphSequencer.AddGlyphToContainer(glyph);
    }

    public void AttackWithGlyph(Glyph glyph)
    {
        glyphToActivate = glyph;
        animator.SetTrigger("OnAttack");
    }
    public void DoAttack()
    {
        Unit caster = this.GetComponent<Unit>();
        if (caster == null) return;
        if (glyphSequencer.gameObject.activeSelf)
        {
            if (glyphToActivate == null)
            {
                glyphSequencer?.EndSequence();
                return;
            }
        }
        Spell spawnedSpell;

        if (!isInTutorial)
        {
            spawnedSpell = glyphToActivate.Activate(caster);
        }
        else
        {
            spawnedSpell = glyphToActivate.CastSpell(caster);
        }
        if (glyphSequencer.gameObject.activeSelf)
        {
            if (spawnedSpell == null)
            {
                glyphSequencer?.EndSequence();
                return;
            }

            glyphSequencer?.RegisterSpell(spawnedSpell);
        }
    }   
}
