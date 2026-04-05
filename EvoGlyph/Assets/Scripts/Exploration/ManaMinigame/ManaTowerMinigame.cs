using System;
using System.Linq;
using UnityEngine;

public class ManaTowerMinigame : MonoBehaviour
{
    public event Action<bool> OnResult;
    public event Action OnFinished;
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
    public bool IsInTutorial = false;
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
        IsInTutorial = tower.IsInTutorial;
        //glyphController.IsInTutorial = tower.IsInTutorial;
        isActive = true;
        gameObject.SetActive(true);
        glyphBoard.GenerateField();
        glyphController.Initialize(this);
        if(!IsInTutorial)
            glyphController.CanDrawGlyph(true);
    }

    public bool GlyphCreated(bool[] glyph)
    {
        if (glyph.SequenceEqual(requiredGlyph.pattern.glyphPattern))
        {
            //SetResult(true);
            if (IsInTutorial)
            {
                OnFinished?.Invoke();
            }
            Debug.Log("Create glyph Success");
            manaTower.RefillMana();
            glyphController.OnEnd();
            return true;
        }
        return false;
    }
    public void SetResult(bool success)
    {
        if (CurrentTryCount >= MaxTries) return;
        tryResult[CurrentTryCount] = success;
        CurrentTryCount++;
        SetTryCountUI();
        if(IsInTutorial)
            OnResult?.Invoke(success);
    }
    public void SetTryCountUI()
    {       
        for (int i = 0; i < CurrentTryCount; i++)
        {
            if (tryResult[i] == false)
                tryCountUI[i].color = Color.red;

            else if (tryResult[i] == true)
                tryCountUI[i].color = Color.green;
        }
    }

    public void TutorialSetTryResultUI(bool success)
    {
        if (!success)
            tryCountUI[0].color = Color.red;

        else
            tryCountUI[0].color = Color.green;
    }

    public void TutorialResetTryResultUI()
    {
        tryCountUI[0].color = Color.white;
    }

    public void ExitMinigame()
    {
        glyphController.CanDrawGlyph(false);
        
        isActive = false;
        glyphBoard.ClearField();
        if(manaTower != null) 
            manaTower.SetPlayerCanMove();
        UIManager.Instance.ShowExplorationUI();
        gameObject.SetActive(false);
    }
}
