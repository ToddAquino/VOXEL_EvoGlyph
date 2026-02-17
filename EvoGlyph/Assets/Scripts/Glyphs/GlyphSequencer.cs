using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GlyphSequencer : MonoBehaviour
{
    public event Action<List<Glyph>> OnEndSequence;
    public InventoryContainer SequencerContainer;
    public PlayerController playerController;
    //public int currentMaxSpellCount;
    //public int currentSpellCount;
    public int currentIndex;

    public void Initialize()
    {
        //currentSpellCount = 0;
        currentIndex = 0;

        foreach (var slot in SequencerContainer.slots)
        {
            SequencerContainer.RemoveItemFromSlot(slot);
        }
    }
    public void BeginCasting()
    {
        //if (SequencerContainer == null) return;
        playerController.controller.GlyphControllerOnEndTurn();
        playerController.AttackWithGlyph(GetGlyphFromIndex(currentIndex));
    }

    public void AddGlyphToContainer(Glyph glyph)
    {
        //if (currentSpellCount >= currentMaxSpellCount) return;
        SequencerContainer.AddItem(glyph);
        //currentSpellCount++;
    }
    public void PlayCurrentMove()
    {
        if (currentIndex >= SequencerContainer.slots.Count || SequencerContainer.slots[currentIndex].Item == null)
        {
            EndSequence();
            return;
        }
        Glyph glyph = SequencerContainer.slots[currentIndex].Item;
        playerController.AttackWithGlyph(glyph);
    }
    public void RegisterSpell(Spell spell)
    {
        spell.OnSpellDespawn += OnMoveFinished;
    }

    public Glyph GetGlyphFromIndex(int index)
    {
        if(index < 0 || index >= SequencerContainer.slots.Count) return null;
        return SequencerContainer.slots[index].Item;
    }
    
    public void OnMoveFinished(Spell spell)
    {
        spell.OnSpellDespawn -= OnMoveFinished;
        currentIndex++;
        PlayCurrentMove();
    }
    public void EndSequence()
    {
        List<Glyph> glyphsInSequence = new List<Glyph>();
        foreach (var slot in SequencerContainer.slots)
        {
            if (slot.Item != null)
                glyphsInSequence.Add(slot.Item);
            SequencerContainer.RemoveItemFromSlot(slot);
        }
        if (BattleManager.Instance != null)
        {
            BattleController battleController = BattleManager.Instance.Controller;
            BattlePhase phase = BattleManager.Instance.Controller.CurrentPhase;
            if (phase == BattlePhase.PlayerAction)
            {
                playerController.GetComponent<Unit>().EndTurn(phase);
            }
        }
        this.gameObject.SetActive(false);
        OnEndSequence?.Invoke(glyphsInSequence);
    }

    //public void SetMaxSpells(int maxSpellCount)
    //{
    //    currentMaxSpellCount = maxSpellCount;
    //    for (int i = 0; i < SequencerContainer.slots.Count; i++)
    //    {
    //        bool shouldBeActive = i < maxSpellCount;
    //        SequencerContainer.slots[i].gameObject.SetActive(shouldBeActive);
    //    }
    //}
}
