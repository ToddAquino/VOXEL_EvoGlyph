using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IUnitController
{
    public PlayerCounterPhaseHandler counterPhaseHandler;
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

    public void OnEndTurn(Unit unit, BattlePhase phase)
    {
        controller.GlyphControllerOnEndTurn();
        //reset glyph sequence
        if (!glyphSequencer.SequencerContainer.slots[0].IsEmpty)
        {
            glyphSequencer.EndSequence();
        }
        StopListenToControllerInput();
        //Tutorial has control on end turn
        if (!isInTutorial)
        {
            switch (phase)
            {
                case BattlePhase.PlayerCounter:
                    BattleManager.Instance.Controller.EndPlayerCounterPhase();
                    break;

                case BattlePhase.PlayerAction:
                    BattleManager.Instance.Controller.EndPlayerActionPhase();
                    break;
            }
        }
    }

    public void OnStartTurn(Unit unit, BattlePhase phase)
    {
        controller.Initialize();
        controller.CanDrawGlyph(true);

        
        glyphSequencer.Initialize();
        switch (phase)
        {
            case BattlePhase.PlayerCounter:
                counterPhaseHandler.StartCounterPhase();
                break;
            case BattlePhase.PlayerAction:
                glyphSequencer.SetMaxSpells(3);
                glyphSequencer.gameObject.SetActive(true);
                ListenToControllerInput();
                break;
        }

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
