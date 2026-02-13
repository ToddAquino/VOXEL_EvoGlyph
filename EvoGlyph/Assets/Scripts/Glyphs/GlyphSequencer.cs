using System;
using UnityEngine;

public class GlyphSequencer : MonoBehaviour
{
    public InventoryContainer SequencerContainer;
    public PlayerController playerController;
    public int currentIndex;
    public void BeginCasting()
    {
        if (SequencerContainer == null) return;
        currentIndex = 0;
        playerController.AttackWithGlyph(GetGlyphFromIndex(currentIndex));
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
        foreach (var slot in SequencerContainer.slots)
        {
            SequencerContainer.RemoveItemFromSlot(slot);
        }
        this.gameObject.SetActive(false);
    }

}
