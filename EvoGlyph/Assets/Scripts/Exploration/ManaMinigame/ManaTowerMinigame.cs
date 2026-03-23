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
    public void Initialize(ManaTower tower)
    {
        if(isActive) return;
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
            Debug.Log("Create glyph Success");
            manaTower.RefillMana();
            glyphController.OnEnd();
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
