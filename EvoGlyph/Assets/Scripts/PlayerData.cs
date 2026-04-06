using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    public UnityEvent OnTomePieceChanged;
    public UnityEvent OnManaChanged;
    [SerializeField] private List<Glyph> unlockedGlyphList = new List<Glyph>();
    private bool pendingDialogueTrigger = false;
   

    [Header("Base Stats")]
    public int maxHP = 100;
    public int baseAttack = 5;

    [Header("Resources")]
    [SerializeField] int maxMana = 5;
    public int CurrentMana;
    public int MaxMana => maxMana;

    [Header("TomePiecesCollected")]
    public int ArcaneTomePieceCount = 0;
    public int FireTomePieceCount = 0;
    public int LightningTomePieceCount = 0;
    public int WaterTomePieceCount = 0;

    [Header("Global Dialogue")]
    public AreaDialogueTrigger globalDialogue;
    [SerializeField] private List<string> targetScenes = new List<string>();
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

    public void AddTomePiece(ElementType TomeType, int PieceCount)
    {
        switch (TomeType)
        {
            case ElementType.Arcane:
                ArcaneTomePieceCount += PieceCount;
                if (ArcaneTomePieceCount > 3)
                {
                    ArcaneTomePieceCount = 3;
                }
                break;

            case ElementType.Fire:
                FireTomePieceCount += PieceCount;
                if (FireTomePieceCount > 3)
                {
                    FireTomePieceCount = 3;
                }
                break;

            case ElementType.Lightning:
                LightningTomePieceCount += PieceCount;
                if (LightningTomePieceCount > 3)
                {
                    LightningTomePieceCount = 3;
                }
                break;

            case ElementType.Water:
                WaterTomePieceCount += PieceCount;
                if (WaterTomePieceCount > 3)
                {
                    WaterTomePieceCount = 3;
                }
                break;
        }
        pendingDialogueTrigger = true;
        SceneManager.sceneLoaded -= OnSceneLoaded; 
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnTomePieceChanged?.Invoke();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!pendingDialogueTrigger) return;

        if (targetScenes.Contains(scene.name))
        {
            StartCoroutine(DelayedDialogueTrigger(0.5f));

            pendingDialogueTrigger = false;

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    private System.Collections.IEnumerator DelayedDialogueTrigger(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (globalDialogue != null)
        {
            globalDialogue.ManualTrigger();
        }
        else
        {
            Debug.LogWarning("globalDialogue is null!");
        }
    }
    public int GetTomePieceCount(ElementType TomeType)
    {
        int count = 0;
        switch (TomeType)
        {
            case ElementType.Arcane:
                count = ArcaneTomePieceCount;
                break;

            case ElementType.Fire:
                count = FireTomePieceCount;
                break;

            case ElementType.Lightning:
                count = LightningTomePieceCount;
                break;

            case ElementType.Water:
                count = WaterTomePieceCount;
                break;
        }

        return count;
    }
}
