using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerData : MonoBehaviour
{
    public UnityEvent OnManaChanged;
    [SerializeField] private List<Glyph> unlockedGlyphList = new List<Glyph>();

    [Header("Base Stats")]
    public int maxHP = 100;
    public int baseAttack = 5;

    [Header("Resources")]
    [SerializeField] int maxMana = 5;
    public int CurrentMana;
    public int MaxMana => maxMana;
    //Glyph Data
    public bool IsUnlocked(Glyph glyph)
    {
        return unlockedGlyphList.Contains(glyph);
    }
    public void UnlockGlyph(Glyph glyph)
    {
        if (!unlockedGlyphList.Contains(glyph))
            unlockedGlyphList.Add(glyph);
    }

    //Mana Data
    public int GetCurrentManaCount()
    {
        return CurrentMana;
    }

    public void RefillMana(int amount)
    {
        CurrentMana += amount;
        CurrentMana = Mathf.Clamp(CurrentMana, 0, maxMana);
        OnManaChanged?.Invoke();
    }

    public void SpendMana(int cost)
    {
        CurrentMana -= cost;
        if(CurrentMana < 0)
            CurrentMana = 0;
        OnManaChanged?.Invoke();
    }
}
