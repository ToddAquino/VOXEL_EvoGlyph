using NUnit.Framework.Internal;
using System.Linq;
using UnityEngine;

public class ManaTowerMinigame : MonoBehaviour
{
    [SerializeField] SpriteRenderer requiredGlyphSpriteRenderer;
    [SerializeField] GlyphBoard glyphBoard;
    [SerializeField] ManaGlyphController glyphController;
    public bool isActive = false;
    Glyph requiredGlyph;
    ManaTower manaTower;
    public int MaxTries = 3;
    public int CurrentTryCount;
    public bool[] tryResult = new bool[] { false, false, false };
    [SerializeField] SpriteRenderer[] tryCountUI;
    public void Initialize(ManaTower tower)
    {
        if(isActive) return;
        foreach (var sprite in tryCountUI)
        {
            sprite.color = Color.white;
        }
        CurrentTryCount = 0;
        tryResult = new bool[] { false, false, false };
        manaTower = tower;
        requiredGlyphSpriteRenderer.sprite = manaTower.RequiredGlyph.GlyphIcon;
        requiredGlyph = tower.RequiredGlyph;
        isActive = true;
        gameObject.SetActive(true);
        Debug.Log("initialize");
        glyphBoard.GenerateField();
        glyphController.Initialize(this);
        glyphController.CanDrawGlyph(true);
        GlyphController.OnCreateGlyph += GlyphCreated;
    }

    public void GlyphCreated(bool[] glyph)
    {
        if (glyph.SequenceEqual(requiredGlyph.pattern.glyphPattern))
        {
            SetResult(true);
            Debug.Log("Create glyph Success");
            manaTower.RefillMana();
            glyphController.OnEnd();
        }
    }
    public void SetResult(bool success)
    {
        if (CurrentTryCount >= MaxTries) return;
        tryResult[CurrentTryCount] = success;
        CurrentTryCount++;
        SetTryCountUI(success);
    }
    public void SetTryCountUI(bool success)
    {       
        for (int i = 0; i < CurrentTryCount; i++)
        {
            if (tryResult[i] == false)
                tryCountUI[i].color = Color.red;

            else if (tryResult[i] == true)
                tryCountUI[i].color = Color.green;
        }
    }

    public void ExitMinigame()
    {
        glyphController.CanDrawGlyph(false);
        GlyphController.OnCreateGlyph -= GlyphCreated;
        
        isActive = false;
        glyphBoard.ClearField();
        if(manaTower != null) 
            manaTower.SetPlayerCanMove();

        gameObject.SetActive(false);
    }
}
