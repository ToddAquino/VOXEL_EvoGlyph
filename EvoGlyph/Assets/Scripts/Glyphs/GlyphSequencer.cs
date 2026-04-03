using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GlyphSequencer : MonoBehaviour
{
    public event Action<List<Glyph>> OnEndSequence;
    public InventoryContainer SequencerContainer;
    public PlayerController playerController;
    public int currentIndex;
    public int currentSpellCount;
    public int manaCount;
    public bool isCastStarted = false;
    [SerializeField] private Glyph currentGlyph;

    private int GetFilledSlotCount()
    {
        int count = 0;

        foreach (var slot in SequencerContainer.slots)
        {
            if (slot.Item != null)
                count++;
        }
        Debug.Log($"ManaCost: {count}");
        return count;
    }
    public void Initialize()
    {
        currentIndex = 0;
        currentSpellCount = 0;
        int maxSpellSlot = SequencerContainer.slots.Count;
        manaCount = GameManager.Instance.PlayerData.GetCurrentManaCount();
        if (manaCount > maxSpellSlot)
            manaCount = maxSpellSlot;

        for (int i = 0; i < SequencerContainer.slots.Count; i++)
        {
            bool shouldBeActive = i < manaCount;
            SequencerContainer.slots[i].gameObject.SetActive(shouldBeActive);
        }

        foreach (var slot in SequencerContainer.slots)
        {
            SequencerContainer.RemoveItemFromSlot(slot);
        }
    }
    public void BeginCasting()
    {
        if(isCastStarted) return;
        isCastStarted = true;
        //Deduct mana based on total mana cost of the spells in sequencer
        int manaCost = GetFilledSlotCount();
        GameManager.Instance.PlayerData.SpendMana(manaCost);

        BattleManager.Instance.controller.GlyphControllerOnEndTurn();
        playerController.AttackWithGlyph(GetGlyphFromIndex(currentIndex));
    }

    public void AddGlyphToContainer(Glyph glyph)
    {
        if (currentSpellCount >= manaCount) return;
        SequencerContainer.AddItem(glyph);
        currentSpellCount++;
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
    public void RegisterSpell(Glyph glyph)
    {
        currentGlyph = glyph;
        currentGlyph.OnGlyphResolved += OnMoveFinished;
    }

    public Glyph GetGlyphFromIndex(int index)
    {
        if(index < 0 || index >= SequencerContainer.slots.Count) return null;
        return SequencerContainer.slots[index].Item;
    }
    
    public void OnMoveFinished()
    {
        currentGlyph.OnGlyphResolved -= OnMoveFinished;
        //currentGlyph = null;
        currentIndex++;
        PlayCurrentMove();
    }
    public void EndSequence()
    {
        isCastStarted = false;
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
                playerController.ActionCastFinished();
            }
        }
        OnEndSequence?.Invoke(glyphsInSequence);
        this.gameObject.SetActive(false);
    }
}
